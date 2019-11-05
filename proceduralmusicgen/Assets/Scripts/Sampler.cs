using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sound generator

public class Sampler : MonoBehaviour
{
    [SerializeField] private StepSequencer _sequencer;

    // The audio file we want to play
    [SerializeField] private AudioClip _audioClip;

    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;

    // The number of voices. Set this higher if you're hearing excessive voice stealing.
    [SerializeField, Range(1, 8)] private int _numVoices = 2;

    // The prefab for the sampler voice, which is just a SamplerVoice component attached to a GameObject
    [SerializeField] private SamplerVoice _samplerVoicePrefab;

    private SamplerVoice[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new SamplerVoice[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }
    }

    private void OnEnable()
    {
        if (_sequencer != null)
        {
            _sequencer.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (_sequencer != null)
        {
            _sequencer.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime, int midiNoteNumber, double duration)
    {
        float pitch = MusicMath.MidiNoteToPitch(midiNoteNumber, MusicMath.MidiNoteC4);
        _samplerVoices[_nextVoiceIndex].Play(_audioClip, pitch, tickTime, _attackTime, duration, _releaseTime);

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }
}
    

