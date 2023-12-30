# Waver
Waver is intended for resampling and converting wave files.
Main purpose is preparing audio files for training KI such as rnnoise.

Copyright 2023 by Wolfgang Wallhäuser.


## Usage
Supported commands:

### analyze
Analyze wav audio files

Example:

`.\Waver.exe analyze -i C:\wav\in\*.wav`

Options:
- -i, --input: Required. Input directory or file(s). Examples: `".\test", "c:\test\*.wav", "c:\test\test.wav"`

### resample
Resample, normalize, trim or split audio files (wav, mp3) to 16bit wav

Examples:

`.\Waver.exe resample -f -m -s 16000 -v -i C:\wav\in\*.* -o C:\wav\out\*.wav`
`.\Waver.exe resample -f -m -s 16000 -n -1  -i .\in\*.wav -d 10 -c 20 -t -f -o .\out\*.wav`

Options:
- -i, --input: Required. Input directory or file(s). Examples: `"c:\test", "c:\test\*.mp3", "c:\test\test.wav"`
- -o, --output: Required. Output filename or mask. Examples: `"c:\test\test.wav", "c:\test\*.wav", "c:\test", "test.wav",
  "*.wav"`
- -m, --mono: Output to mono file (default: stereo resp. original channel count)
- -s, --sample_rate: Output sample rate in Hz. Default: 16000
- -n, --normalization: Normalize output volume to max peek db level. When not given, the volume is unchanged. 
Allowed values: 0 for maximum peek or a negative integer number. Example: `-n -3`
- -d, --split_duration: Split output to split_count file(s) with split_duration (seconds)
- -c, --split_count: Split output to max split_count file(s) with split_duration (seconds). When not given, the whole file is split up.
- -t, --trim: Trim leading silence
- -f, --force: Overwrite existing file.
- -v, --verbose: Set output to verbose messages.

### raw2wav
Convert 16bit raw pcm audio files to wav. The sample rate must match the original sample rate of the existing
raw audio data, no resampling is performed. Add option `-m` resp. `--mono` when the raw audio data is mono.

Example:

`.\Waver.exe raw2wav -f -m -s 16000 -v -i C:\wav\raw\*.raw -o C:\wav\out\*.wav`

Options:
- -i, --input: Required. Input directory or file(s). Examples: `"c:\test", "c:\test\*.raw", "c:\test\test.pcm"`
- -o, --output: Required. Output filename or mask. Examples: `"c:\test\test.wav", "c:\test\*.wav", "c:\test", "test.wav",
  "*.wav"`
- -m, --mono: Output to mono file (default: stereo). Mono / stereo must match the original raw audio format.
- -s, --sample_rate: Output sample rate in Hz. Default: 16000. Sample rate must match the original raw audio format.
- -f, --force: Overwrite existing file.
- -v, --verbose: Set output to verbose messages.

### wav2raw
Convert wav audio files to 16bit pcm raw files. The channel count and sample rate are not changed upon conversion. 

Example:

`.\Waver.exe wav2raw -v -i C:\wav\in\*.wav -o C:\wav\out\*.raw`

Options:
- -i, --input: Required. Input directory or file(s). Examples: `"c:\test", "c:\test\*.wav", "c:\test\test.wav"`
- -o, --output: Required. Output filename or mask. Examples: `"c:\test\test.pcm", "c:\test\*.pcm", "c:\test", "test.pcm",
  "*.raw"`
- -f, --force: Overwrite existing file.

### concat
Concatenates raw pcm files. When the destination file already exists, the input files are appended to the existing file.

Example:

`.\Waver.exe concat -v -i C:\wav\in\*.raw -o C:\wav\out\concat.raw`

Options:
- -i, --input: Required. Input directory or file(s). Examples: `"c:\test", "c:\test\*.raw", "c:\test\test.pcm"`
- -o, --output: Required. Output filename. Examples: `"c:\test\test.pcm", "test.raw"`
- -f, --force: Append existing file.

