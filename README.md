## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Apprentice AAN Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-apprentice-aan-web?branchName=main)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3044&branchName=main)
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=SkillsFundingAgency_das-apprentice-aan-web)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apprentice-aan-web)
[![Confluence Page](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3852894209/AAN+Apprentice+Solution+Architecture)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This web solution is part of Apprentice Ambassador Network (AAN) project. Here the apprentice users can onboard to become ambassadors, find and sign-up for network events, find and collaborate with other ambassadors.

## How It Works
Users are expected to register themselves in the Apprentice portal. Once registered they will have access to on-boarding journey for AAN. Users who are already apprentices will also have access to on-board. Currently the Apprentice portal is using [das-apprentice-login-service](https://github.com/SkillsFundingAgency/das-apprentice-login-service) for authentication. The user registration happens in the [das-apprentice-accounts-web](https://github.com/SkillsFundingAgency/das-apprentice-accounts-web). When running this locally, the launch url should be `https://localhost:7053`.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* An Azure Active Directory account with the appropriate roles as per the [config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-tools-servicebus-support/SFA.DAS.Tools.Servicebus.Support.json).
* The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/ApprenticeAan) should be available either running locally or accessible in an Azure tenancy.
* The [das-apprentice-login-service](https://github.com/SkillsFundingAgency/das-apprentice-login-service) should be available either running locally or accessible in an Azure tenancy.
* The [das-apprentice-accounts-web](https://github.com/SkillsFundingAgency/das-apprentice-accounts-web) should be available either running locally or accessible in an Azure tenancy.

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
  "Version": "1.0",
  "cdn": {
    "url": "https://das-test-frnt-end.azureedge.net"
  }
} 
```

## Technologies
* .NetCore 8.0
* NUnit
* Moq
* FluentAssertions
* RestEase
* MediatR
