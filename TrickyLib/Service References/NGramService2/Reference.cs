﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrickyLib.NGramService2 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TokenSet", Namespace="http://schemas.microsoft.com/research/2009/10/webngram/fanout")]
    [System.SerializableAttribute()]
    public partial class TokenSet : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float backoffField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string cookieField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float[] probabilitiesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] wordsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float backoff {
            get {
                return this.backoffField;
            }
            set {
                if ((this.backoffField.Equals(value) != true)) {
                    this.backoffField = value;
                    this.RaisePropertyChanged("backoff");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string cookie {
            get {
                return this.cookieField;
            }
            set {
                if ((object.ReferenceEquals(this.cookieField, value) != true)) {
                    this.cookieField = value;
                    this.RaisePropertyChanged("cookie");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float[] probabilities {
            get {
                return this.probabilitiesField;
            }
            set {
                if ((object.ReferenceEquals(this.probabilitiesField, value) != true)) {
                    this.probabilitiesField = value;
                    this.RaisePropertyChanged("probabilities");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] words {
            get {
                return this.wordsField;
            }
            set {
                if ((object.ReferenceEquals(this.wordsField, value) != true)) {
                    this.wordsField = value;
                    this.RaisePropertyChanged("words");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://schemas.microsoft.com/research/2011/08/wiab", ConfigurationName="NGramService2.IWiabService")]
    public interface IWiabService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetModels", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetModelsResponse" +
            "")]
        string[] GetModels();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetProbability", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetProbabilityRes" +
            "ponse")]
        float GetProbability(string authorizationToken, string modelUrn, string phrase);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetProbabilities", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetProbabilitiesR" +
            "esponse")]
        float[] GetProbabilities(string authorizationToken, string modelUrn, string[] phrases);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetConditionalPro" +
            "bability", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetConditionalPro" +
            "babilityResponse")]
        float GetConditionalProbability(string authorizationToken, string modelUrn, string phrase);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetConditionalPro" +
            "babilities", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GetConditionalPro" +
            "babilitiesResponse")]
        float[] GetConditionalProbabilities(string authorizationToken, string modelUrn, string[] phrases);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/Generate", ReplyAction="http://schemas.microsoft.com/research/2011/08/wiab/IWiabService/GenerateResponse")]
        TrickyLib.NGramService2.TokenSet Generate(string authorizationToken, string modelUrn, string phraseContext, int maxN, string cookie);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWiabServiceChannel : TrickyLib.NGramService2.IWiabService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WiabServiceClient : System.ServiceModel.ClientBase<TrickyLib.NGramService2.IWiabService>, TrickyLib.NGramService2.IWiabService {
        
        public WiabServiceClient() {
        }
        
        public WiabServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WiabServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WiabServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WiabServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] GetModels() {
            return base.Channel.GetModels();
        }
        
        public float GetProbability(string authorizationToken, string modelUrn, string phrase) {
            return base.Channel.GetProbability(authorizationToken, modelUrn, phrase);
        }
        
        public float[] GetProbabilities(string authorizationToken, string modelUrn, string[] phrases) {
            return base.Channel.GetProbabilities(authorizationToken, modelUrn, phrases);
        }
        
        public float GetConditionalProbability(string authorizationToken, string modelUrn, string phrase) {
            return base.Channel.GetConditionalProbability(authorizationToken, modelUrn, phrase);
        }
        
        public float[] GetConditionalProbabilities(string authorizationToken, string modelUrn, string[] phrases) {
            return base.Channel.GetConditionalProbabilities(authorizationToken, modelUrn, phrases);
        }
        
        public TrickyLib.NGramService2.TokenSet Generate(string authorizationToken, string modelUrn, string phraseContext, int maxN, string cookie) {
            return base.Channel.Generate(authorizationToken, modelUrn, phraseContext, maxN, cookie);
        }
    }
}
