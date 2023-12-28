Waver is used for resampling and converting wave files.
Main purpose: Prepare raw audio files for training rnnoise.

Supported commands:

* analyze: Analyze wav audio files.
  * Example: .\Waver.exe analyze -v -i C:\wav\in\*.wav
* resample: Resample audio files (wav, mp3) to 16bit wav.
  * Example: .\Waver.exe resample -f -m -s 16000 -v -i C:\wav\in\*.* -o C:\wav\out\*.wav
* concat: Concatenate (append) raw pcm audio files.
  * Example: .\Waver.exe concat -v -i C:\wav\in\*.raw -o C:\wav\out\concat.raw
* raw2wav: Convert 16bit raw pcm audio files to wav.
  * Example: .\Waver.exe raw2wav -f -m -s 16000 -v -i C:\wav\raw\*.raw -o C:\wav\out\*.wav
* wav2raw: Convert wav audio files to 16bit pcm raw.
  * Example: .\Waver.exe wav2raw -v -i C:\wav\in\*.wav -o C:\wav\out\*.raw
