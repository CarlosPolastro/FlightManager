// notEqualValidation.js
(function ($) {
    // Add custom validation method to jQuery Validation
    $.validator.addMethod("notequal", function (value, element, params) {
        var otherProperty = $(`#${params.other}`).val();
        return value !== otherProperty;
    }, '');

    // Add unobtrusive adapter to link with ASP.NET Core
    $.validator.unobtrusive.adapters.add("notequal", ["other"], function (options) {
        options.rules["notequal"] = {
            other: options.params.other // Pass the other property's ID
        };
        options.messages["notequal"] = options.message; // Use the error message from the attribute
    });
})(jQuery);
