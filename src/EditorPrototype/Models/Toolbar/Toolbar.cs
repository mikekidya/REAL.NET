﻿using PluginLibrary.MainInterfaces;
using System;
using System.Collections.Generic;

namespace EditorPrototype.Models.Toolbar
{
    public class Toolbar : IToolbar
    {
        public IList<IButton> Buttons => throw new NotImplementedException();

        public void AddButton(ICommand command, string desription, string image)
        {
            throw new NotImplementedException();
        }

        public void AddMenu(IMenu menu)
        {
            throw new NotImplementedException();
        }
    }
}