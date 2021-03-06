﻿using Sitana.Framework.Cs;
using Sitana.Framework.Ui.DefinitionFiles;
using Sitana.Framework.Ui.Views.ButtonDrawables;
using Sitana.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitana.Framework.Ui.Views
{
    public class UiRadioButton: UiButton
    {
        public new static void Parse(XNode node, DefinitionFile file)
        {
            UiButton.Parse(node, file);

            var parser = new DefinitionParser(node);

            file["Value"] = parser.ParseInt("Value");
            file["SelectedValue"] = parser.ParseInt("SelectedValue");

            file["SubValue"] = parser.ParseInt("SubValue");
            file["SelectedSubValue"] = parser.ParseInt("SelectedSubValue");
        }

        public SharedValue<int> SelectedValue { get; private set; }
        public int Value { get; private set; }

        public SharedValue<int> SelectedSubValue { get; private set; }
        public int? SubValue { get; private set; }

        public override ButtonState ButtonState
        {
            get
            {
                if (SubValue.HasValue)
                {
                    if(SubValue.Value != SelectedSubValue.Value)
                    {
                        return base.ButtonState;
                    }
                }

                return base.ButtonState | (SelectedValue.Value == Value ? ButtonState.Checked : ButtonState.None);
            }
        }

        protected override bool Init(object controller, object binding, DefinitionFile definition)
        {
            if (!base.Init(controller, binding, definition))
            {
                return false;
            }

            DefinitionFileWithStyle file = new DefinitionFileWithStyle(definition, typeof(UiRadioButton));

            SelectedValue = DefinitionResolver.GetShared<int>(Controller, Binding, file["SelectedValue"], -1);
            Value = DefinitionResolver.Get<int>(Controller, Binding, file["Value"], 0);

            if(file["SubValue"] != null)
            {
                SubValue = DefinitionResolver.Get<int>(Controller, Binding, file["SubValue"], 0);
                SelectedSubValue = DefinitionResolver.GetShared<int>(Controller, Binding, file["SelectedSubValue"], -1);
            }

            return true;
        }

        protected override void DoAction()
        {
            SelectedValue.Value = Value;

            if(SubValue.HasValue && SelectedSubValue != null)
            {
                SelectedSubValue.Value = SubValue.Value;
            }

            CallDelegate("Click");

            if (_actionSound != null)
            {
                _actionSound.Play();
            }
        }
    }
}
