using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using CelestialHookMod.Common;
using System.Collections.Generic;

namespace CelestialHookMod.Items
{
	internal class CelestialHook : ModItem
	{

        private int mode = 0;

		public override void SetDefaults() 
		{
            Item.CloneDefaults(ItemID.DualHook);
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<PhantasmalHookProjectile>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            List<string> keys = KeybindSystem.CelestialHookSwap.GetAssignedKeys();
            string key = "N/A";
            if (keys.Count > 0) 
            {
                key = keys[0];
            }
            TooltipLine newline = new TooltipLine(Mod, "CHookTutorial", "Press \'" + key + "\' to switch hook mode");
            tooltips.Insert(2, newline);
        }

		public override void AddRecipes() 
		{
			CreateRecipe()
                .AddIngredient<Items.PhantasmalHook>()
				.AddIngredient<Items.SolarHook>()
				.AddIngredient<Items.NebulaHook>()
				.AddIngredient<Items.VortexHook>()
				.AddIngredient<Items.StardustHook>()
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
		
		public override void UpdateInventory(Player player) 
		{
            mode = CelestialHookHandler.GetMode(Item);
            if ((KeybindSystem.CelestialHookSwap.JustPressed))
            {
                mode = (mode + 1) % 5;
                CelestialHookHandler.SetMode(Item, mode);
            }
		}

    }
	
	internal class CelestialHookHandler : ModPlayer
	{
        public int mode = 0;

        public override void PostUpdate() 
		{
			if ((KeybindSystem.CelestialHookSwap.JustPressed))
			{
                for (int i = 0; i < Player.miscEquips.Length; i++)
                {
                    if (Player.miscEquips[i].Name == "Celestial Hook")
                    {
                        mode = GetMode(Player.miscEquips[i]);
                        mode = (mode + 1) % 5;
                        SetMode(Player.miscEquips[i], mode);
                    }
                }
			}
		}

        public static int GetMode(Item hook)
        {
            switch (hook.shootSpeed)
            {
                case 25f:
                    return 0;
                case 20f:
                    return 1;
                case 22.5f:
                    return 2;
                case 17.5f:
                    return 3;
                case 15f:
                    return 4;
            }
            return -1;
        }

        public static void SetMode(Item hook, int m)
        {
            switch (m)
            {
                case 0:
                    hook.CloneDefaults(ItemID.DualHook);
                    hook.shootSpeed = 25f;
                    hook.shoot = ModContent.ProjectileType<PhantasmalHookProjectile>();
                    break;
                case 1:
                    hook.CloneDefaults(ItemID.LunarHook);
                    hook.shootSpeed = 20f;
                    hook.shoot = ModContent.ProjectileType<SolarHookProjectile>();
                    break;
                case 2:
                    hook.CloneDefaults(ItemID.LunarHook);
                    hook.shootSpeed = 22.5f;
                    hook.shoot = ModContent.ProjectileType<NebulaHookProjectile>();
                    break;
                case 3:
                    hook.CloneDefaults(ItemID.LunarHook);
                    hook.shootSpeed = 17.5f;
                    hook.shoot = ModContent.ProjectileType<VortexHookProjectile>();
                    break;
                case 4:
                    hook.CloneDefaults(ItemID.LunarHook);
                    hook.shootSpeed = 15f;
                    hook.shoot = ModContent.ProjectileType<StardustHookProjectile>();
                    break;

            }
        }
	}
	
}

