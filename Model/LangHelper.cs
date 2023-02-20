﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace EasySave.Model;

public class LangHelper
{
    public ResourceManager _rm { get; set; }

    public LangHelper()
    {
        _rm = new ResourceManager("EasySave.Lang.Resources", Assembly.GetExecutingAssembly());
    }

    public string? GetString(string name)
    {
        return _rm.GetString(name);
    }

    public void ChangeLanguageFrench()
    {
        var cultureInfo = new CultureInfo("fr");

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public void ChangeLanguageEnglish()
    {
        var cultureInfo = new CultureInfo("en");

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}
