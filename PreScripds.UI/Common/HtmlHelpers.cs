using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using PreScripds.Infrastructure;
using PreScripds.Infrastructure.Utilities;

namespace PreScripds.UI.Common
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public IDictionary<string, object> htmlAttributes { get; set; }
    }

    public static class HtmlHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this ICollection<T> collection,
            Func<T, string> valueSelector,
            Func<T, string> textSelector, string initialValue = null)
        {
            if (collection == null)
                return new List<SelectListItem>();

            return collection.Select(c =>
            {

                var isSelected = (initialValue == valueSelector(c));
                return new SelectListItem()
                {
                    Text = textSelector(c),
                    Value = valueSelector(c),
                    Selected = isSelected
                };
            });
        }

        //public static IEnumerable<ExtendedSelectListItem> ToDataAttributeListItems<T>(this ICollection<T> collection,
        //   Func<T, string> valueSelector,
        //   Func<T, string> textSelector, Func<T, string> dataSelector, string dataAttrName, string initialValue = null)
        //{
        //    if (collection == null)
        //        return new List<ExtendedSelectListItem>();

        //    return collection.Select(c =>
        //    {

        //        var isSelected = (initialValue == valueSelector(c));

        //        var listItem = new ExtendedSelectListItem()
        //        {
        //            Text = textSelector(c),
        //            Value = valueSelector(c),
        //            htmlAttributes = new Dictionary<string, object> { { dataAttrName, dataSelector(c) } },
        //            Selected = isSelected
        //        };

        //        return listItem;
        //    });
        //}

        //public static string ToJson(this object obj)
        //{
        //    return JsonConvert.SerializeObject(obj, Formatting.None);
        //}


        //public static MvcHtmlString BooleanDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        //{
        //    return htmlHelper.DropDownListFor(expression, new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "Yes", Value = "true"},
        //        new SelectListItem {Text = "No", Value = "false"}
        //    }, "--Select--", htmlAttributes);
        //}

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes, Type enumType)
        {
            var values = Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(e => new SelectListItem() { Value = Convert.ToInt32(e).ToString(), Text = e.GetDisplayName() })
                .ToList();
            return htmlHelper.DropDownListFor(expression, values, "--Select--", htmlAttributes);
        }

        //public static void RemoveFor<TModel>(this ModelStateDictionary modelState,
        //    Expression<Func<TModel, object>> expression)
        //{
        //    string expressionText = ExpressionHelper.GetExpressionText(expression);

        //    foreach (var ms in modelState.ToArray())
        //    {
        //        if (ms.Key.StartsWith(expressionText + ".") || ms.Key == expressionText)
        //        {
        //            modelState.Remove(ms);
        //        }
        //    }
        //}

        //public static MvcHtmlString HiddenTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.TextBoxFor(expression, new { @class = "hidden" });
        //}

        public static T Iff<T>(this HtmlHelper html, bool condition, T trueValue, T falseValue)
        {
            return condition ? trueValue : falseValue;
        }

        public static T Iff<T>(this HtmlHelper html, bool condition, T trueValue)
        {
            return html.Iff<T>(condition, trueValue, default(T));
        }

        //public static MvcHtmlString ExtendedDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, IEnumerable<ExtendedSelectListItem> selectList,
        //    string optionLabel, object htmlAttributes)
        //{
        //    return SelectInternal(htmlHelper, optionLabel, ExpressionHelper.GetExpressionText(expression), selectList,
        //        false /* allowMultiple */, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        //}

        //public static MvcHtmlString MessageDialog(this HtmlHelper htmlHelper, string message, bool isSuccess = true)
        //{
        //    // Convert each ListItem to an <option> tag
        //    StringBuilder modalContentBuilder = new StringBuilder();
        //    modalContentBuilder.AppendFormat("<div class='modal fade in' id='{0}-modal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='false' style='display: none;'>", isSuccess ? "success" : "error");
        //    modalContentBuilder.AppendLine("<div class='modal-dialog'>");
        //    modalContentBuilder.AppendLine("<div class='modal-content'>");
        //    modalContentBuilder.AppendLine("<div class='modal-body'>");
        //    modalContentBuilder.AppendFormat("<div id='message' class='{0}'>{1}</div>", isSuccess ? "successMsg" : "errorMsg", message);
        //    modalContentBuilder.AppendLine("</div>");
        //    modalContentBuilder.AppendLine("<div class='modal-footer'>");
        //    modalContentBuilder.AppendLine("<button type='button' class='' data-dismiss='modal'>Close</button>");
        //    modalContentBuilder.AppendLine("</div>");
        //    modalContentBuilder.AppendLine("</div>");
        //    modalContentBuilder.AppendLine("</div>");
        //    modalContentBuilder.AppendLine("</div>");

        //    return MvcHtmlString.Create(modalContentBuilder.ToString());
        //}

        public static MvcHtmlString MessageDialogRedirect(this HtmlHelper htmlHelper, string message, string ActionName, string ControllerName, bool isSuccess = true)
        {
            // Convert each ListItem to an <option> tag
            //string url = @"redirect('@Url.Action(" + ActionName + ", " + ControllerName + ")')";
            string url = "location.href= buildUrl('" + ControllerName + "/" + ActionName + "');";
            StringBuilder modalContentBuilder = new StringBuilder();
            modalContentBuilder.AppendFormat("<div class='modal fade in' id='{0}-modal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='false' style='display: none;'>", isSuccess ? "success" : "error");
            modalContentBuilder.AppendLine("<div class='modal-dialog'>");
            modalContentBuilder.AppendLine("<div class='modal-content'>");
            modalContentBuilder.AppendLine("<div class='modal-body'>");
            modalContentBuilder.AppendFormat("<div class='{0}'>{1}.</div>", isSuccess ? "successMsg" : "errorMsg", message);
            modalContentBuilder.AppendLine("</div>");
            modalContentBuilder.AppendLine("<div class='modal-footer'>");

            modalContentBuilder.AppendLine("<button id='sucessBtn' type='button' onclick='" + url + "'>OK</button>");
            modalContentBuilder.AppendLine("</div>");
            modalContentBuilder.AppendLine("</div>");
            modalContentBuilder.AppendLine("</div>");
            modalContentBuilder.AppendLine("</div>");
            modalContentBuilder.AppendLine("<script type='text/javascript'>$('#{0}-modal').on('hidden.bs.modal', function () {{ $('#sucessBtn').click(); }}); $('#sucessBtn').click(function(){{ {1} }})</script>".ToFormat(isSuccess ? "success" : "error", url));

            return MvcHtmlString.Create(modalContentBuilder.ToString());
        }
        //private static MvcHtmlString SelectInternal(this HtmlHelper htmlHelper, string optionLabel, string name,
        //    IEnumerable<ExtendedSelectListItem> selectList, bool allowMultiple,
        //    IDictionary<string, object> htmlAttributes)
        //{
        //    string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        //    if (String.IsNullOrEmpty(fullName))
        //        throw new ArgumentException("No name");

        //    if (selectList == null)
        //        throw new ArgumentException("No selectlist");

        //    object defaultValue = (allowMultiple)
        //        ? GetModelStateValue(htmlHelper, fullName, typeof(string[]))
        //        : GetModelStateValue(htmlHelper, fullName, typeof(string));

        //    // If we haven't already used ViewData to get the entire list of items then we need to
        //    // use the ViewData-supplied value before using the parameter-supplied value.
        //    if (defaultValue == null)
        //        defaultValue = htmlHelper.ViewData.Eval(fullName);

        //    if (defaultValue != null)
        //    {
        //        IEnumerable defaultValues = (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue };
        //        IEnumerable<string> values = from object value in defaultValues
        //                                     select Convert.ToString(value, CultureInfo.CurrentCulture);
        //        HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
        //        List<ExtendedSelectListItem> newSelectList = new List<ExtendedSelectListItem>();

        //        foreach (ExtendedSelectListItem item in selectList)
        //        {
        //            item.Selected = (item.Value != null)
        //                ? selectedValues.Contains(item.Value)
        //                : selectedValues.Contains(item.Text);
        //            newSelectList.Add(item);
        //        }
        //        selectList = newSelectList;
        //    }

        //    // Convert each ListItem to an <option> tag
        //    StringBuilder listItemBuilder = new StringBuilder();

        //    // Make optionLabel the first item that gets rendered.
        //    if (optionLabel != null)
        //        listItemBuilder.Append(
        //            ListItemToOption(new ExtendedSelectListItem()
        //            {
        //                Text = optionLabel,
        //                Value = String.Empty,
        //                Selected = false
        //            }));

        //    foreach (ExtendedSelectListItem item in selectList)
        //    {
        //        listItemBuilder.Append(ListItemToOption(item));
        //    }

        //    TagBuilder tagBuilder = new TagBuilder("select")
        //    {
        //        InnerHtml = listItemBuilder.ToString()
        //    };
        //    tagBuilder.MergeAttributes(htmlAttributes);
        //    tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
        //    tagBuilder.GenerateId(fullName);
        //    if (allowMultiple)
        //        tagBuilder.MergeAttribute("multiple", "multiple");

        //    // If there are any errors for a named field, we add the css attribute.
        //    ModelState modelState;
        //    if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
        //    {
        //        if (modelState.Errors.Count > 0)
        //        {
        //            tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
        //        }
        //    }

        //    tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name));

        //    return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        //}

        //internal static string ListItemToOption(ExtendedSelectListItem item)
        //{
        //    TagBuilder builder = new TagBuilder("option")
        //    {
        //        InnerHtml = HttpUtility.HtmlEncode(item.Text)
        //    };
        //    if (item.Value != null)
        //    {
        //        builder.Attributes["value"] = item.Value;
        //    }
        //    if (item.Selected)
        //    {
        //        builder.Attributes["selected"] = "selected";
        //    }
        //    builder.MergeAttributes(item.htmlAttributes);
        //    return builder.ToString(TagRenderMode.Normal);
        //}

        //public static object GetModelStateValue(HtmlHelper htmlHelper, string key, Type destinationType)
        //{
        //    ModelState modelState;
        //    if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
        //    {
        //        if (modelState.Value != null)
        //        {
        //            return modelState.Value.ConvertTo(destinationType, null /* culture */);
        //        }
        //    }
        //    return null;

        //}

        //public static string GetAbsoluteAssetPath(string pubId, string assetPath, string assetName)
        //{
        //    return Path.Combine(ConfigurationManager.AppSettings["AssetsBasePath"],
        //        assetPath, assetName);
        //}

        //public static String ShowAllErrors(this HtmlHelper helper, String key, String cssclass)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (helper.ViewData.ModelState[key] != null)
        //    {
        //        TagBuilder div = new TagBuilder("div");
        //        div.MergeAttribute("class", cssclass);
        //        StringBuilder errorList = new StringBuilder();
        //        foreach (var e in helper.ViewData.ModelState[key].Errors)
        //        {
        //            errorList.Append(e.ErrorMessage + "<br/>");
        //        }
        //        div.SetInnerText(errorList.ToString());
        //        sb.Append(div.ToString());
        //    }
        //    return sb.ToString();
        //}

        //public static string RenderRazorViewToString(this Controller controller, string viewName, object model, IDictionary<string, object> viewDataDictionary)
        //{
        //    controller.ViewData.Model = model;
        //    if (viewDataDictionary.IsCollectionValid())
        //    {
        //        viewDataDictionary.ForEach(x => controller.ViewData[x.Key] = x.Value);
        //    }

        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
        //        var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}

        //public static MvcHtmlString EmbedCss(this HtmlHelper htmlHelper, string path)
        //{
        //    // take a path that starts with "~" and map it to the filesystem.
        //    var cssFilePath = HttpContext.Current.Server.MapPath(path);
        //    // load the contents of that file
        //    try
        //    {
        //        var cssText = System.IO.File.ReadAllText(cssFilePath);
        //        var styleElement = new TagBuilder("style");
        //        styleElement.SetInnerText(cssText);
        //        return MvcHtmlString.Create(styleElement.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        // return nothing if we can't read the file for any reason
        //        return null;
        //    }
        //}

        //public static MvcHtmlString ProgressBar(this HtmlHelper htmlHelper)
        //{
        //    var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
        //    return MvcHtmlString.Create(string.Format(@"<div class='loadingDiv' style='display: none;'><img src='{0}' class='progress-img' /></div>", urlHelper.Content("~/Images/ajax-loader.gif")));
        //}
    }
}


