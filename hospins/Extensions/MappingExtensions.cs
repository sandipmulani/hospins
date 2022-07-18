using AutoMapper;
using hospins.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hospins.Models;

namespace hospins.Extensions
{
    public static class MappingExtensions
    {
        private static IMapper _mapper;

        public static void InitializeMapper(this IMapper mapper)
        {
            _mapper = mapper;
        }

        #region :: User ::
        public static UserModel ToModel(this User entity)
        {
            return _mapper.Map<User, UserModel>(entity);
        }

        public static User ToEntity(this UserModel model)
        {
            return _mapper.Map<UserModel, User>(model);
        }
        #endregion

        #region :: Category ::
        public static Category ToEntity(this CategoryModel model)
        {
            return _mapper.Map<CategoryModel, Category>(model);
        }

        public static CategoryModel ToModel(this Category entity)
        {
            return _mapper.Map<Category, CategoryModel>(entity);
        }
        #endregion

        #region :: SubCategory ::
        public static SubCategory ToEntity(this SubCategoryModel model)
        {
            return _mapper.Map<SubCategoryModel, SubCategory>(model);
        }

        public static SubCategoryModel ToModel(this SubCategory entity)
        {
            return _mapper.Map<SubCategory, SubCategoryModel>(entity);
        }
        #endregion

        #region :: Priority ::
        public static Priority ToEntity(this PriorityModel model)
        {
            return _mapper.Map<PriorityModel, Priority>(model);
        }

        public static PriorityModel ToModel(this Priority entity)
        {
            return _mapper.Map<Priority, PriorityModel>(entity);
        }
        #endregion

        #region :: Logbook ::
        public static Logbook ToEntity(this LogbookModel model)
        {
            return _mapper.Map<LogbookModel, Logbook>(model);
        }

        public static LogbookModel ToModel(this Logbook entity)
        {
            return _mapper.Map<Logbook, LogbookModel>(entity);
        }
        #endregion

        #region :: Designation ::
        public static Designation ToEntity(this DesignationModel model)
        {
            return _mapper.Map<DesignationModel, Designation>(model);
        }

        public static DesignationModel ToModel(this Designation entity)
        {
            return _mapper.Map<Designation, DesignationModel>(entity);
        }
        #endregion

        #region :: Employee ::
        public static Employee ToEntity(this EmployeeModel model)
        {
            return _mapper.Map<EmployeeModel, Employee>(model);
        }

        public static EmployeeModel ToModel(this Employee entity)
        {
            return _mapper.Map<Employee, EmployeeModel>(entity);
        }
        #endregion

        #region :: EmployeeAddress ::
        public static EmployeeAddress ToEntity(this EmployeeAddressModel model)
        {
            return _mapper.Map<EmployeeAddressModel, EmployeeAddress>(model);
        }

        public static EmployeeAddressModel ToModel(this EmployeeAddress entity)
        {
            return _mapper.Map<EmployeeAddress, EmployeeAddressModel>(entity);
        }
        #endregion

        #region :: EmployeeSalarySetup ::
        public static EmployeeSalarySetup ToEntity(this EmployeeSalarySetupModel model)
        {
            return _mapper.Map<EmployeeSalarySetupModel, EmployeeSalarySetup>(model);
        }

        public static EmployeeSalarySetupModel ToModel(this EmployeeSalarySetup entity)
        {
            return _mapper.Map<EmployeeSalarySetup, EmployeeSalarySetupModel>(entity);
        }
        #endregion

        #region :: EmployeeHistory ::
        public static EmployeeHistory ToEntity(this EmployeeHistoryModel model)
        {
            return _mapper.Map<EmployeeHistoryModel, EmployeeHistory>(model);
        }

        public static EmployeeHistoryModel ToModel(this EmployeeHistory entity)
        {
            return _mapper.Map<EmployeeHistory, EmployeeHistoryModel>(entity);
        }

        public static List<EmployeeHistory> ToEntity(this List<EmployeeHistoryModel> model)
        {
            return _mapper.Map< List<EmployeeHistoryModel>, List<EmployeeHistory>>(model);
        }

        public static List<EmployeeHistoryModel> ToModel(this List<EmployeeHistory> entity)
        {
            return _mapper.Map< List<EmployeeHistory>, List<EmployeeHistoryModel>>(entity);
        }
        #endregion

        #region :: EmployeeDocument ::
        public static EmployeeDocument ToEntity(this EmployeeDocumentModel model)
        {
            return _mapper.Map<EmployeeDocumentModel, EmployeeDocument>(model);
        }

        public static EmployeeDocumentModel ToModel(this EmployeeDocument entity)
        {
            return _mapper.Map<EmployeeDocument, EmployeeDocumentModel>(entity);
        }

        public static List<EmployeeDocument> ToEntity(this List<EmployeeDocumentModel> model)
        {
            return _mapper.Map< List<EmployeeDocumentModel>, List<EmployeeDocument>>(model);
        }

        public static List<EmployeeDocumentModel> ToModel(this List<EmployeeDocument> entity)
        {
            return _mapper.Map< List<EmployeeDocument>, List<EmployeeDocumentModel>>(entity);
        }
        #endregion

        #region :: Document Type ::
        public static DocumentType ToEntity(this DocumentTypeModel model)
        {
            return _mapper.Map<DocumentTypeModel, DocumentType>(model);
        }

        public static DocumentTypeModel ToModel(this DocumentType entity)
        {
            return _mapper.Map<DocumentType, DocumentTypeModel>(entity);
        }
        #endregion

        #region :: Salary Type ::
        public static SalaryType ToEntity(this SalaryTypeModel model)
        {
            return _mapper.Map<SalaryTypeModel, SalaryType>(model);
        }

        public static SalaryTypeModel ToModel(this SalaryType entity)
        {
            return _mapper.Map<SalaryType, SalaryTypeModel>(entity);
        }
        #endregion
    }
}
