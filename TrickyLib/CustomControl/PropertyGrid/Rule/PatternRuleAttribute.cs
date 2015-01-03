using System.Text.RegularExpressions;

namespace TrickyLib.CustomControl.PropertyGrid.Rule
{
	/// <summary>
	/// Validate the input data against a Regular Expression
	/// </summary>
	public class PatternRuleAttribute : RuleBaseAttribute
	{
		#region Data Members

		private readonly string _pattern;
        private readonly string _example;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pattern"></param>
		public PatternRuleAttribute( string pattern, string example = "" )
		{
			this._pattern = pattern;
            this._example = example;
		}

		#endregion

		#region Methods - Public

		/// <summary>
		/// Validate the input data
		/// </summary>
		/// <param name="dataObject"></param>
		/// <returns></returns>
		public override bool IsValid( object dataObject )
		{
			this.ErrorMessage = string.Empty;

			if ( dataObject is string )
			{
				string data = (string)dataObject;
				if ( Regex.IsMatch( data, this._pattern ) )
				{
					return ( true );
				}
			}

			this.ErrorMessage = string.Format(
				"The value you entered: {0} doesn't match the given pattern.",
				dataObject );
            
            if(_example != "")
                this.ErrorMessage += "Example: " + _example;

			return ( false );
		}

		#endregion
	}
}

