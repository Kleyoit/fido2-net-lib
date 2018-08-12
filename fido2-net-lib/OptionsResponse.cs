﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;
using static fido2NetLib.Fido2NetLib;

namespace fido2NetLib
{
    public class CredentialCreateOptions
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 
        /// This member contains data about the Relying Party responsible for the request.
        /// Its value’s name member is required.
        /// Its value’s id member specifies the relying party identifier with which the credential should be associated.If omitted, its value will be the CredentialsContainer object’s relevant settings object's origin's effective domain.
        /// </summary>
        [JsonProperty("rp")]
        public Rp Rp { get; set; }

        /// <summary>
        /// This member contains data about the user account for which the Relying Party is requesting attestation. 
        /// Its value’s name, displayName and id members are required.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// Must be generated by the Server (Relying Party)
        /// </summary>
        [JsonProperty("challenge")]
        [JsonConverter(typeof(Base64UrlConverter))]
        public byte[] Challenge { get; set; }

        /// <summary>
        /// This member contains information about the desired properties of the credential to be created. The sequence is ordered from most preferred to least preferred. The platform makes a best-effort to create the most preferred credential that it can.
        /// </summary>
        [JsonProperty("pubKeyCredParams")]
        public List<PubKeyCredParam> PubKeyCredParams { get; set; }

        /// <summary>
        /// This member specifies a time, in milliseconds, that the caller is willing to wait for the call to complete. This is treated as a hint, and MAY be overridden by the platform.
        /// </summary>
        [JsonProperty("timeout")]
        public long Timeout { get; set; }

        // todo: add more members from https://w3c.github.io/webauthn/#dom-publickeycredentialcreationoptions-pubkeycredparams

        private static PubKeyCredParam ES256 = new PubKeyCredParam()
        {
            Type = "public-key",
            Alg = -7
        };

        /// <summary>
        /// This member is intended for use by Relying Parties that wish to express their preference for attestation conveyance.The default is none.
        /// </summary>
        [JsonProperty("attestation")]
        public string Attestation { get; set; } = "none";

        public static CredentialCreateOptions Create(byte[] challenge, Configuration config)
        {
            return new CredentialCreateOptions
            {
                Status = "ok",
                ErrorMessage = string.Empty,
                Challenge = challenge,
                Rp = new Rp("localhost", config.ServerDomain),
                Timeout = config.Timeout,
                PubKeyCredParams = new List<PubKeyCredParam>()
                {
                    ES256 // todo: is this ok?
                }
                
            };
        }
    }

    public class PubKeyCredParam
    {
        /// <summary>
        /// The type member specifies the type of credential to be created.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The alg member specifies the cryptographic signature algorithm with which the newly generated credential will be used, and thus also the type of asymmetric key pair to be generated, e.g., RSA or Elliptic Curve.
        /// </summary>
        [JsonProperty("alg")]
        public long Alg { get; set; }
    }

    public class Rp
    {
        public Rp(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// A human-readable name for the entity. Its function depends on what the PublicKeyCredentialEntity represents:
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// A unique identifier for the Relying Party entity, which sets the RP ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class User
    {

        /// <summary>
        /// todo: Unsure if this is needed
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The user handle of the user account entity.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A human-friendly name for the user account, intended only for display. For example, "Alex P. Müller" or "田中 倫". The Relying Party SHOULD let the user choose this, and SHOULD NOT restrict the choice more than necessary.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}