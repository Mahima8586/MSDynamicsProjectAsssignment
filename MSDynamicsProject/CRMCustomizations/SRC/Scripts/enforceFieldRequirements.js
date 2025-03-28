﻿function enforceFieldRequirements(executionContext)
{
    var formContext = executionContext.getFormContext();
    var preferredContact = formContext.getAttribute("preferredcontactmethodcode").getValue();
    var emailField = formContext.getAttribute("emailaddress1");
    var phoneField = formContext.getAttribute("telephone1");

    if (preferredContact === 2)
    { 
        emailField.setRequiredLevel("required");
        phoneField.setRequiredLevel("none");
    }
    else if (preferredContact === 3)
    { 
        phoneField.setRequiredLevel("required");
        emailField.setRequiredLevel("none");
    }
    else
    { 
        emailField.setRequiredLevel("none");
        phoneField.setRequiredLevel("none");
    }
}

function validateFieldsOnSave(executionContext)
{
    var formContext = executionContext.getFormContext();
    var email = formContext.getAttribute("emailaddress1").getValue();
    var phone = formContext.getAttribute("telephone1").getValue();

    if (!email && !phone)
    {
        alert("Either Email or Phone must be filled before saving.");
        executionContext.getEventArgs().preventDefault(); 
    }
}
