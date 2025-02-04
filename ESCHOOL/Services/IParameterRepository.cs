﻿using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IParameterRepository
    {
        IEnumerable<Parameter> GetParameterAll();
        IEnumerable<Parameter> GetParameterMain(string categoryLevel);
        IEnumerable<Parameter> GetParameterSubID(int categorySubID);
        IEnumerable<Parameter> GetParameterSubIDOnlyTrue(int categorySubID);
        IEnumerable<Parameter> GetParameterSubIDOnlyTrue2(int categorySubID);
        IEnumerable<Parameter> GetParameterDetail(int categoryID, string categoryLevel);
        Parameter GetParameter(int categoryID);
        Parameter GetParameterCategoryName(string name);
        Parameter GetParameterCategoryName2(int categorySubID, string name);
        bool CreateParameter(Parameter parameter);
        bool UpdateParameter(Parameter parameter);
        bool DeleteParameter(Parameter parameter);
        bool ExistParameter(int categoryID);
        bool ExistCategoryName(string categoryName);
        bool ExistCategoryNameSub(int categoryID, string categoryName);
        bool Save();
    }
}
