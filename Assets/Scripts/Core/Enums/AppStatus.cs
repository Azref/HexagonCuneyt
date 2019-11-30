using System;

/// <summary>
/// Please do not remove EDITOR comments in the file. 
/// They used to edit script within editor while creating screens 
////////// </summary>
namespace Assets.Scripts.Core.Enums
{
    [Flags]
    public enum AppStatus
    {
        Blocked = 1 << 0,
        Confirm = 1,
        Home = 2,

        Page1 = 1 << 3,
        Page2 = 1 << 4,
        Page3 = 1 << 5,
        //Pages = Page1 | Page2 | Page3,

        /*EDITOR-BREAKPOINT*/

        All = ~(-1 << 6/*EDITOR-NUMPOINT*/)
    }

}