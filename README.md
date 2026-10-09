# NhxExtraJewels

A private mod for personal use with friends - not published on Thunderstore.

Adds six gems to **Jewelcrafting**, with 54 gem effects and 6 synergies. It's a port of MidnightsFX's **EpicJewels** 1.1.2 with the same effects, power values and spawn rates, but with gems renamed and recolored so they can't be confused with vanilla gems (Jade, Amber) or Jewelcrafting's own gems.

EpicJewels replacement: don't install both. NhxExtraJewels won't load while EpicJewels is installed. Gems found with EpicJewels don't carry over.

Requires **Jewelcrafting** and **Jötunn**, on the server and on every player's game.

## Gems

| Gem | Color | Was in EpicJewels | Item names |
|----|----|----|----|
| Peridot | lime | Jade | `Uncut_peridot_Stone`, `Simple_peridot_Socket`, ... |
| Tiger's Eye | bronze | Amber | `Uncut_tigerseye_Stone`, `Simple_tigerseye_Socket`, ... |
| Turquoise | green-teal | Aquamarine | `Uncut_turquoise_Stone`, `Simple_turquoise_Socket`, ... |
| Rhodolite | raspberry | Garnet | `Uncut_rhodolite_Stone`, `Simple_rhodolite_Socket`, ... |
| Moonstone | silver-blue | Opal | `Uncut_moonstone_Stone`, `Simple_moonstone_Socket`, ... |
| Tanzanite | blue-violet | Amethyst | `Uncut_tanzanite_Stone`, `Simple_tanzanite_Socket`, ... |

Jewelcrafting generates every stage of each gem from its templates, like for its own gems: `Raw_x_Gemstone` (the formation in the world), `Uncut_x_Stone`, `Shattered_x_Crystal`, and `Simple_x_Socket`, `Advanced_x_Socket`, `Perfect_x_Socket`.

### Peridot

|Socket|Effect|Description|
|----|----|----|
|Necklace|Coin Greed|Enemies may drop coins.|
|Tool|Add Chop Damage|Adds woodcutting damage.|
|Shield|Staggering Block|Chance to stagger an enemy when blocking.|
|Weapon, Magic Staves|Add Slash Damage|Adds slash damage to your attacks.|
|Chest|Slash Resistance|Reduces taken slash damage.|
|Legs|Expert Acrobat|Increases jump skill.|
|Cloak|Coin Hoarder|Increases damage based on how many coins you carry.|
|Head|Expert Daggers|Increases knives skill.|
|Trinket|Toxifier|Gain adrenaline from poison around you.|

### Tiger's Eye

|Socket|Effect|Description|
|----|----|----|
|Necklace|Weapon Reduced Stamina|Reduces stamina cost for attacking.|
|Tool|Farmer|Chance to get additional crops when harvesting, picks up nearby crops.|
|Shield|Block Reduce Stamina|Reduces the stamina cost for blocking attacks.|
|Weapon, Magic Staves|Add Blunt Damage|Adds blunt damage to your attacks.|
|Chest|Expert Smasher|Increases maces skill.|
|Legs|Blunt Resistance|Reduces taken blunt damage.|
|Cloak|Expert Spearmaiden|Increases spear skill.|
|Head|Increase Stamina Regen|Increases stamina regeneration rate.|
|Trinket|Adrenal Rewire|Gain adrenaline from damage taken.|

### Turquoise

|Socket|Effect|Description|
|----|----|----|
|Necklace|Water Frenzy|Increases damage when wet.|
|Tool|Wet Worker|Reduces usage stamina cost when wet.|
|Shield|Freezing Guard|Chance to apply frost damage on block.|
|Weapon, Magic Staves|Add Pierce Damage|Adds pierce damage.|
|Chest|Pierce Resistance|Reduces taken pierce damage.|
|Legs|Water Swiftness|Increases run and jog speed when wet.|
|Cloak|Water Resistant|Prevents wetness for a period of time.|
|Head|Expert Sprinter|Increases run skill.|
|Trinket|Soaked Fury|Gain additional adrenaline while wet.|

### Rhodolite

|Socket|Effect|Description|
|----|----|----|
|Necklace|Burning Frenzy|Increases damage when you are on fire.|
|Tool|Add Pickaxe Damage|Adds pickaxe damage.|
|Shield|Burning Guard|Chance to apply fire damage on block.|
|Weapon, Magic Staves|Inferno|Chance to do massive fire damage.|
|Chest|Expert Axemaster|Increases axes skill.|
|Legs|Fire Resistance|Reduces taken fire damage.|
|Cloak|Burning Viking|Increases speed when burning.|
|Head|Expert Mage|Increases elemental and blood magic skills.|
|Trinket|Burning Adrenaline|Gain additional adrenaline while on fire.|

### Moonstone

|Socket|Effect|Description|
|----|----|----|
|Necklace|Reduce Weight|Reduces the total weight of items in your inventory.|
|Tool|Expert Harvester|Increases woodcutting and pickaxe skills.|
|Shield|Retribution|Chance to return a portion of blocked damage.|
|Weapon, Magic Staves|Add Lightning Damage|Adds lightning damage.|
|Chest|Expert Polearms|Increases polearms skill.|
|Legs|Lightning Resistance|Reduces taken lightning damage.|
|Cloak|Cover of Darkness|Summons friendly bats when fighting.|
|Head|Expert Fisher|Increases fishing skill.|
|Trinket|Harvest Adrenaline|Chance to gain adrenaline from harvesting.|

### Tanzanite

|Socket|Effect|Description|
|----|----|----|
|Necklace|Increase Stamina|Increases max stamina.|
|Tool|Eitr Fused|Spend eitr to power up your damage.|
|Shield|Eitr Conversion|Chance to restore eitr when blocking.|
|Weapon, Magic Staves|Add Spirit Damage|Adds spirit damage.|
|Chest|Expert Swordsman|Increases swords skill.|
|Legs|Expert Brawler|Increases fists skill.|
|Cloak|Poison Resistance|Reduces taken poison damage.|
|Head|Increase Eitr|Increases max eitr.|
|Trinket|Eitr Feedback|Chance to restore adrenaline from eitr use.|

The weapon effects also apply to bows and crossbows. The "Expert" effects add flat skill levels, visible on your skills page.

## Synergies

Only the first two synergies show in the synergy panel, but all of them count.

|Name|Requirements|Details|
|----|----|----|
|Combat Spirit|more than 3 moonstone and more than 3 tanzanite, in equal amounts|A spirit wolf joins you in combat for a short time, and returns after a cooldown.|
|Intense Fire|at least 7 rhodolite|You are more fire resistant and have a higher chance to trigger Inferno.|
|Weapon Master|exactly 6 tiger's eye and exactly 6 peridot|Your skill with all weapons is increased.|
|Slippery When Wet|at least 6 turquoise|You are much faster when wet.|
|Waterproof|at least 13 turquoise|You do not get wet.|
|Spellsword|at least 9 tanzanite, 6 moonstone and 3 tiger's eye|Your attacks use eitr to boost damage.|

## Configuration

Gem effects, power values, biome spawn rates and synergies can be changed through Jewelcrafting's YAML configuration, like Jewelcrafting's own gems. The defaults are in `Assets/ExtraJewelsConfig.yaml` in the source.

The mod's own config file, `NhxExtraJewels.cfg`, has two client settings:

- `EnableItemTooltipDisplay`: shows the damage added by gem effects on the tooltip of your equipped weapons (default on)
- `EnableDebugMode`: debug logging (default off)

## Existing worlds

Gem formations only spawn in newly explored areas. For areas you've already explored, Jewelcrafting's console command `jewelcrafting generate` adds formations (see Jewelcrafting's "Adding the mod to existing worlds").

## Credits

- **EpicJewels** by MidnightsFX (MIT No Attribution license), which this mod is a port of: https://github.com/MidnightsFX/Epic_Jewels
- **Jewelcrafting** by Smoothbrain, whose API creates the gems
