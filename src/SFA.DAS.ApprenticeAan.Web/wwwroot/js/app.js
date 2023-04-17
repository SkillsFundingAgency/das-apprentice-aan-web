function AutoComplete(selectField) {
    this.selectElement = selectField
}

AutoComplete.prototype.init = function() {
    this.autoComplete()
}

AutoComplete.prototype.getSuggestions = function(query, updateResults) {
    let results = [];
    let apiUrl = "/locations?query=" + query
    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function() {
      if (xhr.readyState === 4) {
        let jsonResponse = JSON.parse(xhr.responseText);
        results = jsonResponse.map(function (result) {
          return result
        });
        updateResults(results);
      }
    }
    xhr.open("GET", apiUrl, true);
    xhr.send();
}

AutoComplete.prototype.onConfirm = function(option) {
  // Populate form fields with selected option
  document.getElementById("OrganisationName").value = option.organisationName
  document.getElementById("AddressLine1").value = option.addressLine1
  document.getElementById("AddressLine2").value = option.addressLine2
  document.getElementById("Town").value = option.town
  document.getElementById("County").value = option.county
  document.getElementById("Postcode").value = option.postcode

}

function inputValueTemplate(result) {
  return result && [result.organisationName, result.addressLine1, result.town, result.postcode].filter(element => element).join(', ')
}

function suggestionTemplate (result) {
  return result && [result.organisationName, result.addressLine1, result.town, result.postcode].filter(element => element).join(', ')
}

AutoComplete.prototype.autoComplete = function() {
    let that = this
    accessibleAutocomplete.enhanceSelectElement({
        selectElement: that.selectElement,
        minLength: 2,
        autoselect: false,
        defaultValue: '',
        displayMenu: 'overlay',
        placeholder: '',
        source: that.getSuggestions,
        showAllValues: false,
        confirmOnBlur: false,
        onConfirm: that.onConfirm,
        templates: {
          inputValue: inputValueTemplate,
          suggestion: suggestionTemplate
        }
    });
}

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
      return nodes.forEach(callback)
    }
    for (let i = 0; i < nodes.length; i++) {
      callback.call(window, nodes[i], i, nodes);
    }
  }

let autoCompletes = document.querySelectorAll('[data-module="autoComplete"]')

nodeListForEach(autoCompletes, function (autoComplete) {
  new AutoComplete(autoComplete).init()
})
