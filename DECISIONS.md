2026-11-11
Did the Shooter and Charger enemies with Gemini, Patroller already exist in my game
Change 1: i redid the scripts and Inspector of enemies because they didnt get and do damage
Change2: After my 2 freinds tried to play i got 2 errors, 1st bug of showing up Pause menu at random time while pressing some buttons, and 2nd bug with player "stuck" on ground and cant jumping anymore, until u jump in wall again. It was mistakes that i fixed in Inspector window
Change 3: enemies working but they are too easy to pass, so i will change their cooldown and maybe add hp

I added data saves for enemies and hazards
I had to change all my sprites to prefabs: enemies, bombs, boss, tools
There was a problem with restarting everytime at Level 3, even though i did level in rght orders 123, needed to fix LevelSpawner, because when i finished the 1st level it didnt came back

2026-25-11
Initial volume control used linear scaling, making 50% slider values nearly inaudible.

Replaced `AudioSource.PlayClipAtPoint` in `Coin.cs` with `AudioManager.PlayPickup()` so every sound routes through the `SFX` AudioMixer group.
Repeated jump and hit SFX sounded monotonous. Added randomized pitch (`0.92–1.08`) and multi-clip array support in AudioManager.
Configured `DontDestroyOnLoad` and active track comparison in `AudioManager` so level music crossfades from menu music 1s and continues seamlessly across level transitions without restarting.