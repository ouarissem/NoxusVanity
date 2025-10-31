using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent;

namespace NoxusVanity.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class NoxusHead : ModItem
    {
        public override LocalizedText DisplayName => Language.GetOrRegister(Mod.GetLocalizationKey($"{Name}.DisplayName"), () => "Noxus Mask");
        public override LocalizedText Tooltip => Language.GetOrRegister(Mod.GetLocalizationKey($"{Name}.Tooltip"), () => "A mask radiating dark void energy...");

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = 7;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<NoxusPlayer>().wearingNoxus = true;
        }
    }

    public class NoxusPlayer : ModPlayer
    {
        public bool wearingNoxus;

        public override void ResetEffects()
        {
            wearingNoxus = false;
        }
    }

    public class NoxusAuraLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            var np = player.GetModPlayer<NoxusPlayer>();
            if (!np.wearingNoxus)
                return;

            Effect effect = ModContent.Request<Effect>("NoxusVanity/Effects/NoxusAura").Value;
            effect.Parameters["time"].SetValue((float)Main.time * 0.03f);
            effect.Parameters["intensity"].SetValue(0.8f);
            effect.Parameters["color"].SetValue(new Vector3(0.5f, 0.1f, 1f));

            Vector2 position = player.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, effect, Main.GameViewMatrix.TransformationMatrix);

            Texture2D tex = TextureAssets.Extra[49].Value;
            Main.spriteBatch.Draw(tex, position, null, Color.White * 0.6f, 0f, tex.Size() / 2f, 0.8f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Lighting.AddLight(player.Center, 0.4f, 0.1f, 0.6f);
        }
    }
}
