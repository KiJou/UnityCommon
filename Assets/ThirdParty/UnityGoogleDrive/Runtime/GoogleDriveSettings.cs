﻿// Copyright 2017-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace UnityGoogleDrive
{
    /// <summary>
    /// Project-specific Google Drive settings resource.
    /// </summary>
    public class GoogleDriveSettings : ScriptableObject
    {
        public const string FULL_ACCESS_SCOPE = "https://www.googleapis.com/auth/drive";
        public const string READONLY_ACCESS_SCOPE = "https://www.googleapis.com/auth/drive.readonly";
        public const string REQUEST_CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string CODE_CHALLENGE_METHOD = "S256";
        public const int UNAUTHORIZED_RESPONSE_CODE = 401;

        /// <summary>
        /// Google Drive API application credentials used to authorize requests.
        /// </summary>
        public AuthCredentials AuthCredentials { get { return authCredentials; } }
        /// <summary>
        /// Scope of access to the user's Google Drive the app will request.
        /// </summary>
        public string AccessScope { get { return accessScope; } }
        /// <summary>
        /// HTML page shown to the user when loopback response is received.
        /// </summary>
        public string LoopbackResponseHtml { get { return loopbackResponseHtml; } }

        [SerializeField] private AuthCredentials authCredentials = null;
        [SerializeField] private string accessScope = FULL_ACCESS_SCOPE;
        [SerializeField] private string loopbackResponseHtml = "<html><h1>Please return to the app.</h1></html>";

        /// <summary>
        /// Retrieves settings from the project resources.
        /// </summary>
        /// <param name="silent">Whether to suppress error when settings resource is not found.</param>
        public static GoogleDriveSettings LoadFromResources (bool silent = false)
        {
            var settings = Resources.Load<GoogleDriveSettings>("GoogleDriveSettings");

            if (!settings && !silent)
            {
                Debug.LogError("UnityGoogleDrive: Settings file not found. " +
                    "Use 'Edit > Project Settings > Google Drive Settings' to create a new one.");
            }

            return settings;
        }
    }
}
