//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scantegra.Core.Entities
{
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9154")]
	public enum ImportFile_ProcessingStatus
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ComplexTransformation = 4,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ImportComplete = 11,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ImportPass1 = 9,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ImportPass2 = 10,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		LookupTransformation = 5,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		NotStarted = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		OwnerTransformation = 7,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Parsing = 2,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		ParsingComplete = 3,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		PicklistTransformation = 6,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		PrimaryKeyTransformation = 12,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		TransformationComplete = 8,
	}
}