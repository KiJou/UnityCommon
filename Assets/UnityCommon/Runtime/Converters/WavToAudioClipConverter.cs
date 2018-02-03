﻿using System;
using UnityEngine;

/// <summary>
/// Converts <see cref="byte[]"/> raw data of a .wav audio file to <see cref="AudioClip"/>.
/// Only PCM16 44100Hz stereo wavs are supported.
/// </summary>
public class WavToAudioClipConverter : IRawConverter<AudioClip>
{
    public RawDataRepresentation[] Representations { get { return new RawDataRepresentation[] {
        new RawDataRepresentation("wav", "audio/wav")
    }; } }

    public AudioClip Convert (byte[] obj)
    {
        var floatArr = Pcm16ToFloatArray(obj);
        var audioClip = AudioClip.Create("Generated WAV Audio", floatArr.Length / 2, 2, 44100, false);
        audioClip.SetData(floatArr, 0);
        return audioClip;
    }

    public object Convert (object obj)
    {
        return Convert(obj as byte[]);
    }

    private static float[] Pcm16ToFloatArray (byte[] input)
    {
        // PCM16 wav usually has 44 byte headers, though not always. 
        // https://stackoverflow.com/questions/19991405/how-can-i-detect-whether-a-wav-file-has-a-44-or-46-byte-header
        const int HEADER_SIZE = 44;
        var inputSamples = input.Length / 2; // 16 bit input, so 2 bytes per sample.
        var output = new float[inputSamples];
        var outputIndex = 0;
        for (var n = HEADER_SIZE; n < inputSamples; n++)
        {
            short sample = BitConverter.ToInt16(input, n * 2);
            output[outputIndex++] = sample / 32768f;
        }
        return output;
    }
}