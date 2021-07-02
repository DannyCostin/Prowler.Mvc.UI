# Prowler.Mvc.UI
Collection of ASP.NET MVC UI Custom Controls similar functionality as Telerik UI for ASP.NET MVC

Provides components with flexible data binding, api, events, appearance customization through templates, accessibility.

Dependency jquery, prowler-mvc.js and prowler-mvc.css.

Check the sample app for documentation on how to use the controls


# Donation
If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate?hosted_button_id=79D39PJ2VKELW)

# Version 1.0.1
Construct the look and feel of the dropdown list

Build a dropdown list in View with Prowler Html helper

```@using Prowler.Mvc.UI```

```<script type="text/javascript" src="~/Scripts/jquery-3.4.1.min.js"></script>```\
```<script type="text/javascript" src="~/Scripts/prowler-mvc.js"></script>```\
```<link rel="stylesheet" href="~/Content/prowler-mvc.css"/>```

```
@(Html.Prowler().DropDownList()
                .Name("Product")
                .BindTo(Model.ProductDataSource)
                .DataTextField(nameof(Product.Name))
                .DataValueField(nameof(Product.Id))
                .SelectedIndex(0)
                .Render()
)
```
Dropdown List main functionality:

- Data Binding
- Grouping(rearrange dropdown items in a group based on a group by key and label)
- Client filtering(adds a search box on the dropdown allowing client side search)
- Server filtering (adds a search box on the dropdown allowing server side search)
- Multiselect(allows multiselect with or without checkbox control)
- Customizing templates (change the appearance of the the dropdown items with binding support)
- Events(attach events to dropdown list changes ex: Dropdown Open, Dropdown Selected Changed)

A series of API methods are available allowing control manipulation from java script

- Disable dropdown
- Enable dropdown
- Get selected dropdown value
- Data Bind model from URL
- Open/Close dropdown
- Set dropdown selected item
