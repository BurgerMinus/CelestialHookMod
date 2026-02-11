using Terraria.ModLoader;

namespace CelestialHookMod.Common
{
	public class KeybindSystem : ModSystem
	{
		public static ModKeybind PhantasmalHookRetract { get; private set; }
		public static ModKeybind CelestialHookSwap { get; private set; }

		public override void Load() {
			PhantasmalHookRetract = KeybindLoader.RegisterKeybind(Mod, "Recall Phantasmal Hook", "P");
			CelestialHookSwap = KeybindLoader.RegisterKeybind(Mod, "Switch Celestial Hook Mode", "G");
		}

		public override void Unload() {
			PhantasmalHookRetract = null;
			CelestialHookSwap = null;
		}
	}
}
