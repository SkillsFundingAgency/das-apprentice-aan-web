## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Apprentice AAN Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-apprentice-aan-web?branchName=main)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3044&branchName=main)
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=SkillsFundingAgency_das-apprentice-aan-web)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apprentice-aan-web)

This web solution is part of Apprentice Ambassador Network (AAN) project. Here the apprentice users can onboard to become ambassadors, find and sign-up for network events, find and collaborate with other ambassadors.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* An Azure Active Directory account with the appropriate roles as per the [config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-tools-servicebus-support/SFA.DAS.Tools.Servicebus.Support.json).
* The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/AdminAan) should be available either running locally or accessible in an Azure tenancy.
* The Inner API [das-aan-hub-api](https://github.com/SkillsFundingAgency/das-aan-hub-api) should be available either running locally or accessible in an Azure tenancy.

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apprentice-aan-web/SFA.DAS.ApprenticeAan.Web.json)

In the web project, if not exist already, add `AppSettings.Development.json` file with following content:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.ApprenticeAan.Web",
  "EnvironmentName": "LOCAL",
  "ResourceEnvironmentName": "LOCAL",
  "cdn": {
    "url": "https://das-test-frnt-end.azureedge.net"
  }
} 
```

## Technologies
* .NetCore 6.0
* NUnit
* Moq
* FluentAssertions
* RestEase
* MediatR
