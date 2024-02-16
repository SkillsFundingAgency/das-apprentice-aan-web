## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Apprentice AAN Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-apprentice-aan-web?repoName=SkillsFundingAgency%2Fdas-apprentice-aan-web&branchName=refs%2Fpull%2F67%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3044&repoName=SkillsFundingAgency%2Fdas-apprentice-aan-web&branchName=refs%2Fpull%2F67%2Fmerge)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=SkillsFundingAgency_das-apprentice-aan-web)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apprentice-aan-web)

[![Confluence Page](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3852894209/AAN+Apprentice+Solution+Architecture)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## Description
This web solution is part of Apprentice Ambassador Network (AAN) project. Here the apprentice users can onboard to become ambassadors, find and sign-up for network events, find and collaborate with other ambassadors.

## How It Works
Users are expected to register themselves in the apprentice portal. Once registered they will have access to on-boarding journey for AAN. Users who are already apprentices will also have access to on-board. 
When running this locally, with stub sign-in enabled, the launch url should be `https://localhost:5003/accounts/v6gr7d/`.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* Optionally an Azure Active Directory account with the appropriate roles.
* The Outer API [das-apim-endpoints](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/EmployerAan) should be available either running locally or accessible in an Azure tenancy.

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
  "Version": "1.0",
  "cdn": {
    "url": "https://das-at-frnt-end.azureedge.net"
  },
  "ResourceEnvironmentName": "LOCAL",
  "StubAuth": true
} 
```

## Technologies
* .NetCore 8.0
* NUnit
* Moq
* FluentAssertions
* RestEase
* MediatR