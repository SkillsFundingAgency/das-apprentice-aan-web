## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _SFA.DAS.ApprenticeAan.Web_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

_Update these badges with the correct information for this project. These give the status of the project at a glance and also sign-post developers to the appropriate resources they will need to get up and running_

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/_projectname_?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=564&projectKey=_projectKey_)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/_pageurl_)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)


## 🚀 Installation

### Pre-Requisites

_Add the pre-requisites needed to successfully run the project so that new developers know how they are to setup their development environment_

_For Example_
```
* A clone of this repository
* A code editor that supports Azure functions and .NetCore 3.1
* A CosmosDB instance or emulator
* An Azure Active Directory account with the appropriate roles as per the [config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-tools-servicebus-support/SFA.DAS.Tools.Servicebus.Support.json)
* The [das-audit](https://github.com/SkillsFundingAgency/das-audit) API available either running locally or accessible in an Azure tenancy    
```
### Config

_Add details of the configuration required to successfully run the project. Adding in the config structure from the das-employer-config repo will help new developers understand what the config looks like and detailing the row keys and partition keys of any config rows will make it obvious where the config needs to be for the project to find it. Adding any further config which does not live in das-employer-config will also assist new developers to get the project running._

> _If you do add config directly to the README you will be required to keep it up-to-date with any changes made to it in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config), for this reason it is suggested that you also provide links to the config in that respoitory so that the latest changes are always available_

_For Example_
```
This utility uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config).

* A connection string for either the Apprenticeship Services ASB namespace or a namespace you own for development
* A CosmosDB connection string for either the Apprenticeship Service instance CosmosDB or a CosmosDB you own for development (you can use the emulator)
* Configure the [das-audit](https://github.com/SkillsFundingAgency/das-audit) project as per [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-audit/SFA.DAS.AuditApiClient.json)
* Add an appsettings.Development.json file
    * Add your connection strings for CosmosDB and ASB to the relevant sections of the file
* The CosmosDB will be created automatically if it does not already exist and the credentials you are connected with have the appropriate rights within the Azure tenant otherwise it will need to be created manually using the details in the config below under `CosmosDbSettings`.
```
AppSettings.Development.json file
```json
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
    "ConfigNames": "SFA.DAS.ApprenticeAan.Web",
    "EnvironmentName": "LOCAL",
    "Version": "1.0",
    "APPINSIGHTS_INSTRUMENTATIONKEY": ""
  }  
```

Azure Table Storage config

Row Key: SFA.DAS.ApprenticeAan.Web_1.0

Partition Key: LOCAL

Data:

```json
{
  "BaseUrl": "localhost:5001",
  "UserIdentitySettings":{
    "RequiredRole": "Servicebus Admin", 
    "UserSessionExpiryHours": 24,
    "UserRefreshSessionIntervalMinutes": 5,
    "NameClaim": "name"
  },
  "CosmosDbSettings":{
    "Url": "",
    "AuthKey": "",
    "DatabaseName": "QueueExplorer",
    "CollectionName": "Session",
    "Throughput": 400,
    "DefaultCosmosOperationTimeout": 55,
    "DefaultCosmosInterimRequestTimeout": 2
  }
}
```

## 🔗 External Dependencies

_Add details of any external dependencies that are required for the project to run, this could be details of authentication providers, API's or stubs/test harnesses._

_For Example_
```
* This utility uses the [das-audit](https://github.com/SkillsFundingAgency/das-audit) Api to log changes
```

## Technologies

_List the key technologies in-use in the project. This will give an indication as to the skill set required to understand and contribute to the project_

_For Example_
```
* .NetCore 3.1
* Azure Functions V3
* CosmosDB
* REDIS
* NLog
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
```

## 🐛 Known Issues

_Add any known issues with the project_

_For Example_

```
* Fails when built under VS2019
```