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
    document.getElementById("Longitude").value = option.longitude
    document.getElementById("Latitude").value = option.latitude

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

// Select Submit
function SelectSubmit(selectField) {
    this.selectField = selectField;
    this.form = selectField.closest("form");
}

SelectSubmit.prototype.init = function () {
    if (!this.selectField) {
        return;
    }
    this.selectField.addEventListener("change", () => {
        this.form.submit();
    });
};

const selectSubmits = document.querySelectorAll('[data-module="selectSubmit"]');
nodeListForEach(selectSubmits, function (selectSubmit) {
    console.log(selectSubmit);
    new SelectSubmit(selectSubmit).init();
});

//Location search autocomplete
const locationInputs = document.querySelectorAll(".location-search-autocomplete");
const apiUrl = "/locations/search";

if (locationInputs.length > 0) {
    for (let i = 0; i < locationInputs.length; i++) {
        const input = locationInputs[i];
        const container = document.createElement("div");
        const withinSelect = document.getElementById("within");

        container.className = "das-autocomplete-wrap";
        container.dataset.trackUserSelected = input.dataset.trackUserSelected;
        input.parentNode.replaceChild(container, input);

        const getSuggestions = async (query, updateResults) => {
            const results = [];
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    let results = JSON.parse(xhr.responseText);
                    results = results.map(function (r) {
                        return r.name;
                    });
                    updateResults(results);
                }
            };
            xhr.open("GET", `${apiUrl}?query=${query}`, true);
            xhr.send();
        };

        accessibleAutocomplete({
            element: container,
            id: input.id,
            name: input.name,
            defaultValue: input.value,
            displayMenu: "overlay",
            showNoOptionsFound: false,
            minLength: 2,
            source: getSuggestions,
            placeholder: "",
            confirmOnBlur: false,
            autoselect: true,
            onConfirm: () => {
                const trackSelection = input.dataset.trackUserSelected;
                if (trackSelection) {
                    const hiddenField = document.getElementById(trackSelection);
                    if (hiddenField) {
                        hiddenField.value = "true";
                    }
                }
                if (withinSelect) {
                    if (withinSelect.value === "all") {
                        withinSelect.value = "10";
                    }
                }
            },
        });
    }

    const autocompleteInputs = document.querySelectorAll(".autocomplete__input");
    if (autocompleteInputs.length > 0) {
        for (let i = 0; i < autocompleteInputs.length; i++) {
            const autocompleteInput = autocompleteInputs[i];
            autocompleteInput.setAttribute("autocomplete", "new-password");
        }
    }
}