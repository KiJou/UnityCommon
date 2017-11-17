﻿// Copyright 2017 Elringus (Artyom Sovetnikov). All Rights Reserved.

namespace UnityGoogleDrive
{
    using System.Collections.Generic;
    
    namespace Data
    {
        /// <summary>
        /// A list of generated file IDs which can be provided in create requests.
        /// </summary>
        public class GeneratedIds : ResourceData
        {
            /// <summary>
            /// Identifies what kind of resource this is. Value: the fixed string "drive#generatedIds".
            /// </summary>
            public override string Kind { get { return "drive#generatedIds"; } }
            /// <summary>
            /// The IDs generated for the requesting user in the specified space.
            /// </summary>
            public List<string> Ids { get; private set; }
            /// <summary>
            /// The type of file that can be created with these IDs.
            /// </summary>
            public string Space { get; private set; }
        }
    }
    
}
