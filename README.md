# JIT-Access-Management

## NOTE

As of v1.0.20230609.1 Access Profiles are restricted to only the users they have been shared with.
If you are upgrading from previous version you need to share your Access Profiles with users.
See [Share Access Profiles with users](https://github.com/jenschristianschroder/JIT-Access-Management#Share-Access-Profile-with-users)

## Introduction

Just-In-Time Access Management Solution for Power Platform

Allow Makers to request Just-In-Time access to Power Platform Environments based on configured Access Profiles.

![Access Request Details screen shot](/docs/images/Access-Request-Details.png)

Access Profiles can be configured to require Approval and/or Justification.

All Access Requests are logged (audit) in Dataverse include timeline of Access Request Activities.

## Prerequisites

### Creator Kit

The JIT Access Management Solution requires that the **Creator Kit** is installed in the environment before the solution is imported.

Creator Kit: https://learn.microsoft.com/en-us/power-platform/guidance/creator-kit/overview

### Azure AD Application Registration

The JIT Access Management Solution relies on an **Azure AD Application Registration** to grant and revoke access to environments.

You must have an Azure AD Application Registration with permissions to use the Dataverse (Dynamics CRM) API.

The Azure AD Application Registration must be added as an Application User (S2S app) with System Administrator security role in the environments that are configured for JIT Access.

### Audit

Audit needs to be enabled in the environment for auditing of Access Profiles and Access Requests to be enabled.

## Installation

1. Download the managed solution from the assets in the latest release: https://github.com/jenschristianschroder/JIT-Access-Management/releases.
2. Import the solution into your environment.

## Setup

1. Launch the **JIT Access Management** Model-Driven app.
2. Select the **Setup** area in bottom left corner.
3. Create a new **JIT Access Management Setup** record.
4. Give the record a name.
5. Enter the **Tenant**. This can be either tenant id or domain name
6. Enter the **Client Id** of the Azure Application Registration to be used for granting and revoking access.
7. Enter the **Client Secret** of the Azure Application Registration to be used for granting and revoking access.
8. Select the **JIT Access Management** area in bottom left corner.

## Configuration

### Configure Access Profiles

1. Create a new **Access Profile**.
   1. Enter a **Name**. The Name is displayed to users when creating Access Requests. It is recommended to name the Access Profile similar to "[Environment Name] [Security Role] [Duration]" (Production System Administrator 10 min).
   2. Enter the **Environment Id** for the environment for which the Access Profile grants access.
   3. Enter the name of the **Security Role**.
   4. Enter a **Description**. The description will be displayed to users requesting access.
   5. **Save** the Access Profile record
2. Once the Access Profile is saved the Get-Environment-Details flow is triggered. This flow will gather the required environment details, update the Access Profile and set the Status Reason to Active.

### Configure Access Profile Approval Process

1. Select the Access Profile for which you want to enable the Approval Process.
2. Click the **Access Profile** > **Approval** > **Enable Approval** button in the command bar.
3. Once the **Approval Form** has loaded, select the **Approval Type** of the Approval Process you want to apply to the Access Profile.
4. Add the users who should be part of the Approval process.

### Share Access Profile with users

By default, an **Access Profile** is not shared with any users. An Access Management administrator needs to share the **Access Profile** with relevant teams or users for users to be able to request access via the profile.

**Access Profiles** should be shared with the **Read** and **Append To** privileges.

For guidance on how to share a record see the official documentation here : [Share rows with a user or team](https://learn.microsoft.com/en-us/power-apps/user/share-row).

## Share the JIT Access Request Canvas App with users

1. The JIT Access Request Canvas App should be shared to users with the **Access Management User** security role.

## Request Access

1. Launch the JIT Access Request Canvas App.
2. Click **New Request**.
3. **Select the Access Profile** you for which you wish to request access.
4. Enter the required information.
5. Click **Submit**.

### JIT Access Request Demo

[![JIT Access Request Demo Video](/docs/images/Access-Request-Details-Demo.png)](https://youtu.be/t7_zFoi50ok)
