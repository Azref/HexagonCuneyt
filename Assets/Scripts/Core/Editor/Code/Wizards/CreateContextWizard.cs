using JetBrains.Annotations;
using UnityEditor;

namespace Assets.Scripts.Core.Editor.Code.Wizards
{
  public class CreateContextWizard : ScriptableWizard
  {
    public string Name;

    [MenuItem("Assets/Code/Context")]
    [UsedImplicitly]
    private static void CreateWizard()
    {
      DisplayWizard("Add Context Panel", typeof(CreateContextWizard), "Add");
    }

    [UsedImplicitly]
    private void OnWizardCreate()
    {
      if (string.IsNullOrEmpty(Name))
        return;

      if (!CodeUtilities.HasSelectedFolder())
      {
        var template = Template.Build(TemplateType.Context).Name(Name);
        template.Save();
        Template.Build(TemplateType.Root).Name(Name).Import(template.Ns).Save();

        AssetDatabase.Refresh();
        return;
      }

      Template.Build(TemplateType.Context).Name(Name).Save();
      Template.Build(TemplateType.Root).Name(Name).Save();

      AssetDatabase.Refresh();
    }
  }
}