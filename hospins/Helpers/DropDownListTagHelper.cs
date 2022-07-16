using hospins.Infrastructure;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using hospins.Repository.Infrastructure;
using System.ComponentModel;

namespace hospins.Helpers
{
    [HtmlTargetElement("drop-down-list", Attributes = ModeAttributeName)]
    public class DropDownListTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string IdAttributeName = "asp-id";
        private const string ModeAttributeName = "asp-mode";
        private const string IsOtherAttributeName = "asp-other";
        private const string IsDefaultAttributeName = "asp-default";
        //private const string IsDataLiveSearch = "live-search";
        private const string ClassAttribute = "asp-class";
        private const string FromEnum = "bind-from-enum";
        private const string Multiple = "asp-multiple";
        private const string WhereClause = "asp-where";
        private const string CustomOptionList = "asp-custome-options";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(ClassAttribute)]
        public string Class { get; set; }

        [HtmlAttributeName(FromEnum)]
        public bool IsFromEnum { get; set; } = false;

        [HtmlAttributeName(ModeAttributeName)]
        public string Mode { get; set; }

        [HtmlAttributeName(IsOtherAttributeName)]
        public bool? IsOther { get; set; }

        [HtmlAttributeName(IsDefaultAttributeName)]
        public string IsDefault { get; set; }

        [HtmlAttributeName(Multiple)]
        public bool IsMultiple { get; set; } = false;

        [HtmlAttributeName(WhereClause)]
        public string Where { get; set; }

        [HtmlAttributeName(CustomOptionList)]
        public List<SelectListItem> CustomOptions { get; set; }

        //[HtmlAttributeName(IsDataLiveSearch)]
        //public string IsDataLive { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ViewContext == null)
                return;
            // this.Items = obj.GetList(ConfigurationSettings.DBConnection);
            output.SuppressOutput();
            output.Content.Clear();
            output.Content.AppendHtml(this.GenerateDropDownList());
        }

        private IHtmlContent GenerateDropDownList()
        {
            TagBuilder tb = new TagBuilder("select");
            tb.AddCssClass("form-control drpdwn " + this.Class);
            if (this.For?.Metadata?.IsRequired ?? false)
            {
                var modelAttributesProperty = this.For.Metadata.GetType().GetProperty("Attributes");
                var modelAttributes = modelAttributesProperty?.GetValue(this.For.Metadata) as ModelAttributes;
                var _attributes = modelAttributes?.PropertyAttributes?.FirstOrDefault();
                var pi = _attributes?.GetType()?.GetProperty("ErrorMessage");
                var ErrorMessage = pi?.GetValue(_attributes) as string;

                tb.Attributes.Add("data-val", "true");
                tb.Attributes.Add("data-val-required", ErrorMessage ?? "Required.");
            }

            if (IsMultiple)
            {
                tb.Attributes.Add("title", IsDefault);
                tb.Attributes.Add("multiple", "multiple");
                this.IsDefault = string.Empty;
            }

            //tb.AddCssClass("selectpicker");
            if (this.For == null)
            {
                tb.GenerateId(this.Id, "");
                tb.Attributes.Add("Name", this.Id);
            }
            else
            {
                tb.GenerateId(this.For.Name, "_");
                //tb.Attributes.Add("Id", this.For.Name.Replace(".", "_"));
                //tb.Attributes.Add("Id", this.For.Name.Replace(".", "_").Replace("[", "_").Replace("]", "_"));
                tb.Attributes.Add("Name", this.For.Name);
            }

            List<SelectListItem> Items;
            if (IsFromEnum)
            {
                Items = InvokeMethod("hospins.Infrastructure.EnumHelper", this.Mode);
            }
            else
            {
                if (string.IsNullOrEmpty(this.Mode))
                {
                    Items = new List<SelectListItem>();
                    if (!string.IsNullOrWhiteSpace(this.IsDefault))
                        Items.Add(new SelectListItem() { Value = "", Text = this.IsDefault });
                }
                else
                {
                    DropdownConfig obj = new DropdownConfig(this.Mode, "", -1, this.Where);
                    Items = obj.GetList(ConfigurationSettings.DBConnection, this.IsDefault);
                }
            }

            //add custom options
            if(CustomOptions?.Count > 0)
                Items.AddRange(CustomOptions);

            foreach (SelectListItem item in Items)
                tb.InnerHtml.AppendHtml(this.GenerateDropDownListItem(item));
            if (IsOther == true)
            {
                TagBuilder tb1 = new TagBuilder("option");
                tb1.Attributes.Add("Value", "-1");
                tb1.InnerHtml.AppendHtml("Others");
                tb.InnerHtml.AppendHtml(tb1);
            }
            return tb;
        }

        private IHtmlContent GenerateDropDownListItem(SelectListItem item)
        {
            TagBuilder tb = new TagBuilder("option");
            tb.Attributes.Add("Value", item.Value);
            tb.InnerHtml.AppendHtml(item.Text);
            if ((this.GetValue() ?? "").Split(",").Contains(item.Value))
                tb.Attributes.Add("selected", "selected");
            return tb;
        }

        protected string GetValue()
        {
            if (this.For == null)
                return null;
            if (this.ViewContext.ModelState.TryGetValue(this.For.Name, out ModelStateEntry modelState) && !string.IsNullOrEmpty(modelState.AttemptedValue))
                return modelState.AttemptedValue;

            if (this.IsMultiple)
            {
                return string.Join(",", (List<int>)this.For.Model);
            }
            return this.For?.Model?.ToString();
        }

        public static List<SelectListItem> InvokeMethod(string typeName, string methodName)
        {
            try
            {
                Type calledType = Type.GetType(typeName);
                return (List<SelectListItem>)calledType.InvokeMember(
                                methodName,
                                BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                                null,
                                null,
                                new Object[] { string.Empty });
            }
            catch (System.Exception ex)
            {
                ex.SetLog("InvokeMethod :: " + typeName + "." + methodName);
                return new List<SelectListItem>();
            }
        }
    }
}
