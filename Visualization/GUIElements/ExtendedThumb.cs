using pgmpm.Model.PetriNet;
using System.Windows.Controls.Primitives;

namespace pgmpm.Visualization.GUIElements
{
    public class ExtendedThumb : Thumb
    {
        public string InternName { get; set; }
        public Node BaseNode { get; set; }
    }
}