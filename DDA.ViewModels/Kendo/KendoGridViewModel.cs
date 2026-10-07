using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.ViewModels.Kendo
{
    public class KendoGridViewModel
    {
        private const string VersionNumber = "1.0.750"; //** Revised

        public KendoGridViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            GridName = "grid";
            ExportToExcelActionName = "ExportGridToExcel";
            ExportToPdfActionName = "ExportToPdf";
            AddNewActionName = "Add";
            ViewActionName = "View";
            EditActionName = "Edit";
            DeleteActionName = "Delete";
            IdQueryStringName = "q";
            IdViewModuleName = "Id";
            IdViewModuleIsHidden = true;
            AddNewResourceKey = "KendoGrid_AddNew";
            EditButtonResourceKey = "KendoGrid_EditButtonText";
            DeleteButtonResourceKey = "KendoGrid_DeleteButtonText";
            ViewButtonResourceKey = "KendoGrid_ViewButtonText";
            CustomButtonResourceKey = "KendoGrid_CustomButtonText";
            DeleteMessageResourceKey = "KendoGrid_DeleteMessage";
            ExportToExcelButtonResourceKey = "KendoGrid_ExportToExcel";
            ExportToPdfButtonResourceKey = "KendoGrid_ExportToPDF";
            AddButtonJsFunctionName = "addPopupAction";
            EditButtonJsFunctionName = "editPopupAction";
            ViewButtonJsFunctionName = "viewPopupAction";
            DeleteButtonJsFunctionName = "deletePopupAction";
            ColumnWidth = 230;
            ActionsColumnWidth = 230;
            Height = "500px";
            Selectable = true;
            Scrollable = true;
            Sortable = true;
            Pageable = true;
            AutoBind = true;
            Total = 0;
            TwoStateDefaultButtonColor = "#d3ffde"; //** Green
            TwoStateChangedButtonColor = "#ffd3d3"; //** Red
            EnableCustomSelectAll = false;
            EnableCustomSelect = false;
            EnableCustomSelectAllJSName = "";
            EnableCustomSelectJSName = "";
            EnableCustomSelectAllService = "";
            EnableCustomSelectJSService = "";
            EnableCustomRowBound = false;
            CustomRowBoundName = "";
            DefualtPageSize = 10;
            //FixedActionButtonsFirstColumn = true;
            //EnableFixedActionButtons = true;
        }

        public string Version { get { return VersionNumber; } }

        #region General
        /// <summary>
        /// Kendo grid's total records for paging
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Kendo grid's name.
        /// </summary>
        public string GridName { get; set; }
        /// <summary>
        /// Kendo grid's height. (Example: 300px)
        /// </summary>
        public string Height { get; set; }
        /// <summary>
        /// The action's name that kendo will use to retrieve data while paging, filtering, sorting..etc.
        /// </summary>
        public string FillDataActionName { get; set; }
        /// <summary>
        /// Controller's name.
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// The property that represents the ID in object/entity.
        /// </summary>
        public string IdViewModuleName { get; set; }
        /// <summary>
        /// The peroperty that represents the ID in object/entity is hidden or not.
        /// </summary>
        public bool IdViewModuleIsHidden { get; set; }
        /// <summary>
        /// The querystring parameter name.
        /// </summary>
        public string IdQueryStringName { get; set; }
        /// <summary>
        /// Default column's width in grid.
        /// </summary>
        [Obsolete("This is no more used, use KendoGridColumnWidth attribute instead.")]
        public int ColumnWidth { get; set; }
        /// <summary>
        /// Default column's width in grid.
        /// </summary>
        public int ActionsColumnWidth { get; set; }
        /// <summary>
        /// Resource type that Kendo will retrieve resources from.
        /// </summary>
        public Type ResourceType { get; set; }
        /// <summary>
        /// Add button resource key name.
        /// </summary>
        public string AddNewResourceKey { get; set; }
        /// <summary>
        /// Add button resource key name.
        /// </summary>
        public string EditButtonResourceKey { get; set; }
        /// <summary>
        /// Add button resource key name.
        /// </summary>
        public string DeleteButtonResourceKey { get; set; }
        /// <summary>
        /// Add button resource key name.
        /// </summary>
        public string ViewButtonResourceKey { get; set; }
        /// <summary>
        /// Add button resource key name.
        /// </summary>
        public string CustomButtonResourceKey { get; set; }
        /// <summary>
        /// Static Text for Custom Button (Not from resource)
        /// </summary>
        public string CustomButtonTextStatic { get; set; }
        /// <summary>
        /// Export to pdf button resource key name.
        /// </summary>
        public string ExportToPdfButtonResourceKey { get; set; }
        /// <summary>
        /// Export to excel button resource key name
        /// </summary>
        public string ExportToExcelButtonResourceKey { get; set; }
        /// <summary>
        /// Delete message resource key name.
        /// </summary>
        public string DeleteMessageResourceKey { get; set; }


        public int DefualtPageSize { get; set; }
        /// <summary>
        /// Kendo's data.
        /// </summary>
        public DataTable Data { get; set; }
        public string DetailsTemplateName { get; set; }
        public bool UseReadParameter { get; set; }
        [Obsolete("Property not used")]
        public string ReadParameterValue { get; set; }
        [Obsolete("Property not used")]
        public string SecondReadParameterValue { get; set; }
        [Obsolete("Property not used")]
        public string ThirdReadParameterValue { get; set; }
        public bool AutoBind { get; set; }
        public bool ShowIconOnActionButtons { get; set; }
        #endregion
        #region Buttons
        public bool ShowRowSelectCheckBox { get; set; }
        public bool ShowExportToPdfButton { get; set; }
        public bool ShowExportToExcelButton { get; set; }
        public bool ShowViewButton { get; set; }
        public bool ShowEditButton { get; set; }
        public bool ShowDeleteButton { get; set; }
        public bool ShowAddNewButton { get; set; }
        public bool ShowCustomButton { get; set; }
        [Obsolete("Use ShowCustomButton")]
        public bool ShowCustomOnRowButton { get; set; }
        #endregion
        #region Actions
        #region Server-Side Actions
        public string ExportToPdfActionName { get; set; }
        public string ExportToExcelActionName { get; set; }
        public string AddNewActionName { get; set; }
        public string ViewActionName { get; set; }
        public string EditActionName { get; set; }
        public string DeleteActionName { get; set; }
        public string CustomButtonActionName { get; set; }
        #endregion
        #region Client-Side Actions
        public string ReadJsFunctionName { get; set; }
        public string AddButtonJsFunctionName { get; set; }
        public string EditButtonJsFunctionName { get; set; }
        public string ViewButtonJsFunctionName { get; set; }
        public string DeleteButtonJsFunctionName { get; set; }
        public string CustomButtonJsFunctionName { get; set; }

        //** Two state color configuration.
        public string TwoStateDefaultButtonColor { get; set; }
        public string TwoStateChangedButtonColor { get; set; }

        //** Edit button change text depending on other value.
        public bool EnableTwoStateTextForEditButton { get; set; }
        public string TwoStateTextPropertyNameForEditButton { get; set; }
        public string TwoStateTextPropertyValueForEditButton { get; set; }
        public string TwoStateTextResourceKeyForEditButton { get; set; }

        //** View button change text depending on other value.
        public bool EnableTwoStateTextForViewButton { get; set; }
        public string TwoStateTextPropertyNameForViewButton { get; set; }
        public string TwoStateTextPropertyValueForViewButton { get; set; }
        public string TwoStateTextResourceKeyForViewButton { get; set; }

        //** Delete button change text depending on other value.
        public bool EnableTwoStateTextForDeleteButton { get; set; }
        public string TwoStateTextPropertyNameForDeleteButton { get; set; }
        public string TwoStateTextPropertyValueForDeleteButton { get; set; }
        public string TwoStateTextResourceKeyForDeleteButton { get; set; }

        //** Custom button change text depending on other value.
        public bool EnableTwoStateTextForCustomButton { get; set; }
        public string TwoStateTextPropertyNameForCustomButton { get; set; }
        public string TwoStateTextPropertyValueForCustomButton { get; set; }
        public string TwoStateTextResourceKeyForCustomButton { get; set; }

        //Custom Properties for Select CheckBox
        public bool EnableCustomSelectAll { get; set; }
        public bool EnableCustomSelect { get; set; }
        public string EnableCustomSelectAllJSName { get; set; }
        public string EnableCustomSelectJSName { get; set; }
        public string EnableCustomSelectAllService { get; set; }
        public string EnableCustomSelectJSService { get; set; }

        //Custom Row Bound 
        public bool EnableCustomRowBound { get; set; }
        public string CustomRowBoundName { get; set; }

        [Obsolete("Use CustomButtonJsFunctionName")]
        public string CustomOnRowButtonJsFunctionName { get; set; }
        #endregion
        #endregion
        #region Features
        #region Client-Side Features
        public bool CallJsFunctionOnAddNewButton { get; set; }
        public bool CallJsFunctionOnEditButton { get; set; }
        public bool CallJsFunctionOnViewButton { get; set; }
        public bool CallJsFunctionOnDeleteButton { get; set; }
        public bool CallJsFunctionOnCustomButton { get; set; }

        [Obsolete("Use CallJsFunctionOnAddNewButton")]
        public bool ShowPopupOnAddNewButton { get; set; }
        [Obsolete("Use CallJsFunctionOnEditButton")]
        public bool ShowPopupOnEditButton { get; set; }
        [Obsolete("Use CallJsFunctionOnViewButton")]
        public bool ShowPopupOnViewButton { get; set; }
        [Obsolete("Use CallJsFunctionOnDeleteButton")]
        public bool ShowPopupOnDeleteButton { get; set; }
        [Obsolete("Use CallJsFunctionOnCustomButton")]
        public bool ShowPopupOnCustomButton { get; set; }
        #endregion

        /// <summary>
        /// Adds filter options to the grid.
        /// </summary>
        public bool Filterable { get; set; }
        /// <summary>
        /// Adds row select option to the grid.
        /// </summary>
        public bool Selectable { get; set; }
        /// <summary>
        /// Adds scrolling option to the grid.
        /// </summary>
        public bool Scrollable { get; set; }
        /// <summary>
        /// Makes grid groupable.
        /// </summary>
        public bool Groupable { get; set; }
        /// <summary>
        /// Adds sort option to the grid.
        /// </summary>
        public bool Sortable { get; set; }
        /// <summary>
        /// Adds pagination option to the grid.
        /// </summary>
        public bool Pageable { get; set; }
        /// <summary>
        /// Adds ability to navigate through grid's cells using keyboard arrows.
        /// </summary>
        public bool Navigatable { get; set; }
        /// <summary>
        /// Makes grid editable.
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// Adds additional filter options to the grid on each column as sub menu.
        /// </summary>
        public bool EnableColumnMenu { get; set; }
        /// <summary>
        /// Enables actions column (view, edit and delete) fixed.
        /// </summary>
        public bool EnableFixedActionButtons { get; set; }
        /// <summary>
        /// Placing actions column (view, edit and delete) as first column.
        /// <para>EnableFixedActionButtons won't work with this option if it's set to false.</para> 
        /// </summary>
        public bool FixedActionButtonsFirstColumn { get; set; }
        /// <summary>
        /// Enable Static Text for Custom Button
        /// </summary>
        public bool EnableCustomButtonStaticText { get; set; }
        #endregion
    }
}
