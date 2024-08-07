using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace StorageControls
{
    [TemplatePart( Name = "Part_TextBlock" , Type = typeof(TextBlock) ) ]

    public class Boon : RangeBase
    {
        private TextBlock _textBlock;




        public Boon()
        {
            DefaultStyleKey = typeof( Boon );
        }




        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _textBlock = GetTemplateChild( "Part_TextBlock" ) as TextBlock;

            base.OnApplyTemplate();

            if ( _textBlock != null )
            {
                _textBlock.Text = "Apply Boon";
            }
        }




        protected override void OnMinimumChanged(double oldMinimum , double newMinimum)
        {
            base.OnMinimumChanged( oldMinimum , newMinimum );
        }




        protected override void OnMaximumChanged(double oldMaximum , double newMaximum)
        {
            base.OnMaximumChanged( oldMaximum , newMaximum );
        }




        protected override void OnValueChanged(double oldValue , double newValue)
        {
            OnValueChanged( this );

            base.OnValueChanged( oldValue , newValue );
        }




        private void OnValueChanged(DependencyObject d)
        {
            var boon = (Boon)d;

            if ( _textBlock != null )
            {
                _textBlock.Text = "Boon " + Value.ToString();
            }
        }
    }
}
