using System;

namespace TrickyLib.CustomControl.PropertyGrid.Data
{
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public abstract class ListAttribute : Attribute
	{
	}
}