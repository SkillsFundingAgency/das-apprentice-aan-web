function AutoComplete(selectField) {
    this.selectElement = selectField
    this.selectedStandardIdSelectId = this.selectElement.id || null
}

AutoComplete.prototype.init = function() {
    this.autoComplete()
}

AutoComplete.prototype.getSuggestions = function(query, updateResults) {
    var results = [];
    var apiUrl = "https://localhost:7054/locations?query=" + query
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function() {
      if (xhr.readyState === 4) {
        var jsonResponse = JSON.parse(xhr.responseText);
        results = jsonResponse.addresses.map(function (result) {
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
  console.log(option)
  document.getElementById("OrganisationName").value = option.organisationName
  document.getElementById("AddressLine1").value = option.addressLine1
  document.getElementById("AddressLine2").value = option.addressLine2
  document.getElementById("Town").value = option.town
  document.getElementById("County").value = option.county
  document.getElementById("Postcode").value = option.postcode

}

function inputValueTemplate (result) {
  return result && result.addressLine1 + ', ' + result.town + ', ' + result.postcode
}

function suggestionTemplate (result) {
  return result && '<strong>' + result.organisationName + '</strong> ' + result.addressLine1 + ', ' + result.town + ', ' + result.postcode
}

AutoComplete.prototype.autoComplete = function() {
    var that = this
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
    for (var i = 0; i < nodes.length; i++) {
      callback.call(window, nodes[i], i, nodes);
    }
  }

var autoCompletes = document.querySelectorAll('[data-module="autoComplete"]')

nodeListForEach(autoCompletes, function (autoComplete) {
  new AutoComplete(autoComplete).init()
})
