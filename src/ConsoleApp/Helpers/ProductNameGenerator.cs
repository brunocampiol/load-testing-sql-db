namespace ConsoleApp.Helpers;

public static class ProductNameGenerator
{
    public static List<string> GenerateAllProducts()
    {
        var result = new List<string>();
        result.AddRange(GenerateScrews());
        result.AddRange(GeneratePipes());
        result.AddRange(GeneratePaintSupplies());
        result.AddRange(GenerateInks());
        result.AddRange(GenerateBuildingMaterials());
        result.AddRange(GenerateAutomotiveParts());
        result.AddRange(GenerateTopWear());
        result.AddRange(GenerateShoes());
        result.AddRange(GeneratePants());
        result.AddRange(GenerateGemstones());
        return result;
    }

    private static IEnumerable<string> GenerateScrews()
    {
        string[] items = {
            "Drywall screw", "Eye screw", "Wood screw", "Machine screw", "Sheet metal screw",
            "Lag screw", "Deck screw", "Self-tapping screw", "Concrete screw", "Set screw",
            "Thumb screw", "Security screw", "Socket head cap screw", "Pan head screw",
            "Flat head screw", "Truss head screw", "Hex head screw", "Shoulder screw",
            "Chipboard screw", "Twinfast screw",
            "Hex bolt", "Carriage bolt", "Eye bolt", "U-bolt", "Anchor bolt", "J-bolt",
            "Flange bolt", "Lag bolt", "T-bolt", "Elevator bolt", "Plow bolt", "Step bolt",
            "Hex nut", "Lock nut", "Wing nut", "Cap nut", "Flange nut", "T-nut", "Coupling nut",
            "Square nut", "Acorn nut", "Jam nut", "Slotted nut", "Castle nut", "Keps nut",
            "Flat washer", "Lock washer", "Fender washer", "Sealing washer", "Spring washer",
            "Blind rivet", "Solid rivet", "Drive rivet", "Split rivet", "Tubular rivet",
            "Cotter pin", "Dowel pin", "Spring pin", "Taper pin", "Groove pin"
        };
        string[] materials = {
            "Steel", "Stainless steel", "Brass", "Aluminum", "Bronze", "Titanium", "Copper",
            "Nickel", "Zinc", "Alloy steel"
        };
        string[] sizes = {
            "55mm x 8g", "60mm x 10g", "80mm x 12g", "100mm x 18g", "4.2x40 (8x1.1/2)",
            "4.2x50 (8x2)", "4.2x55 (8x2.1/4)", "4.5x60 (9x2.1/2)", "4.5x70 (9x3)",
            "5.5x50 (12x2)", "5.5x19 (12x3/4)", "5.5x25 (12x1)", "5.5x32 (12x1.1/4)",
            "5.5x38 (12x1.1/2)", "5.5x70 (12x2.3/4)", "3mm x 10mm", "3mm x 15mm",
            "3mm x 20mm", "4mm x 10mm", "4mm x 15mm", "4mm x 20mm", "6mm x 20mm",
            "6mm x 30mm", "8mm x 40mm", "10mm x 50mm", "1/4in x 1in", "1/4in x 2in",
            "3/8in x 1in", "3/8in x 2in", "1/2in x 2in", "1/2in x 4in"
        };
        
        foreach (var item in items)
            foreach (var material in materials)
                foreach (var size in sizes)
                    yield return $"{item} {material} {size}";
    }

    private static IEnumerable<string> GeneratePipes()
    {
        string[] pipes = {
            "Pipe", "Flexible hose", "Hydraulic hose", "Garden hose",
            "Electrical conduit", "PVC conduit", "Metal duct", "Flex duct", "Cable channel",
            "Drainage channel", "Cable trunking", "Protective sleeve", "Threaded rod", "Steel rod",
            "Flat bar", "Round bar", "Handrail", "Guard rail"
        };
        string[] materials = {
            "PVC", "Copper", "PEX", "Galvanized steel", "Black steel", "Stainless steel",
            "Brass", "CPVC", "ABS", "Cast iron", "Polyethylene", "Ductile iron", "Concrete"
        };
        string[] sizes = {
            "1/2 in x 10 ft", "3/4 in x 10 ft", "1 in x 10 ft", "1-1/4 in x 10 ft",
            "1-1/2 in x 10 ft", "2 in x 10 ft", "3 in x 10 ft", "4 in x 10 ft", "6 in x 10 ft",
            "8 in x 10 ft", "15mm x 3m", "22mm x 3m", "28mm x 3m", "32mm x 6m", "40mm x 6m",
            "50mm x 6m", "63mm x 6m"
        };
        
        foreach (var pipe in pipes)
            foreach (var material in materials)
                foreach (var size in sizes)
                    yield return $"{pipe} {material} {size}";
    }

    private static IEnumerable<string> GeneratePaintSupplies()
    {
        string[] items = {
            "Paint brush", "Paint roller", "Paint tray", "Paint tray liner", "Paint can opener",
            "Paint stir stick", "Drop cloth", "Painter's tape", "Putty knife", "Scraper",
            "Sanding sponge", "Sandpaper", "Paint edger", "Paint shield", "Extension pole",
            "Paint sprayer", "Paint bucket", "Paint grid", "Paint cup", "Mixing paddle",
            "Caulking gun", "Spackle knife", "Dust mask", "Gloves", "Coveralls"
        };
        string[] materials = {
            "Nylon", "Polyester", "Foam", "Microfiber", "Lambswool", "Plastic", "Metal",
            "Steel", "Aluminum", "Wood", "Paper", "Cotton", "Canvas", "Rubber"
        };
        string[] sizes = {
            "1 in", "2 in", "3 in", "4 in", "6 in", "9 in", "12 in", "18 in", "Quart",
            "Gallon", "5-gallon", "Small", "Medium", "Large", "Extra large", "25 ft",
            "50 ft", "100 ft"
        };
        
        foreach (var item in items)
            foreach (var material in materials)
                foreach (var size in sizes)
                    yield return $"{item} {material} {size}";
    }

    private static IEnumerable<string> GenerateInks()
    {
        string[] types = {
            "Dye-based", "Pigment-based", "Solvent-based", "UV-curable", "Gel", "Ballpoint",
            "Fountain", "Acrylic", "Watercolor", "India ink", "Calligraphy ink", "Iron gall ink",
            "Sepia ink", "Textile ink", "Screen printing ink", "Marker ink", "Stamp pad ink",
            "Tattoo ink", "Printer ink", "Plotter ink", "Whiteboard ink", "Highlighter ink",
            "Permanent ink", "Washable ink", "UV-reactive ink", "Thermochromic ink",
            "Invisible ink", "Metallic ink", "Fluorescent ink"
        };
        string[] colors = {
            "Black", "White", "Gray", "Silver", "Charcoal", "Ivory", "Beige", "Tan", "Brown",
            "Chocolate", "Espresso", "Taupe", "Camel", "Copper", "Bronze", "Gold", "Red",
            "Crimson", "Scarlet", "Burgundy", "Maroon", "Ruby", "Cherry", "Rose", "Coral",
            "Salmon", "Vermilion", "Magenta", "Pink", "Fuchsia", "Blush", "Peach", "Orange",
            "Tangerine", "Apricot", "Amber", "Rust", "Pumpkin", "Copper", "Ochre", "Yellow",
            "Lemon", "Canary", "Gold", "Mustard", "Daffodil", "Butter", "Cream", "Green",
            "Emerald", "Olive", "Lime", "Mint", "Jade", "Chartreuse", "Moss", "Forest", "Pine",
            "Seafoam", "Teal", "Turquoise", "Aqua", "Cyan", "Blue", "Navy", "Azure", "Cobalt",
            "Sapphire", "Sky Blue", "Cerulean", "Indigo", "Denim", "Prussian Blue",
            "Turquoise Blue", "Purple", "Violet", "Lavender", "Lilac", "Mauve", "Plum",
            "Orchid", "Amethyst", "Eggplant", "Bordeaux", "Wine", "Mahogany", "Sand",
            "Sienna", "Umber", "Slate", "Steel Blue", "Periwinkle", "Powder Blue", "Ice Blue",
            "Mint Green", "Sea Green", "Kelly Green", "Hunter Green", "Apple Green",
            "Spring Green", "Fern Green", "Jungle Green", "Sunflower", "Banana", "Honey",
            "Sandstone", "Desert", "Khaki", "Army Green", "Copper Red", "Firebrick",
            "Brick Red", "Blood Red", "Rosewood", "Mulberry", "Raspberry", "Strawberry",
            "Pumpkin Orange", "Carrot Orange", "Amber Yellow", "Sunset Orange",
            "Burnt Orange", "Lime Green", "Pistachio", "Shamrock", "Clover", "Basil",
            "Moss Green", "Royal Blue", "Baby Blue", "Electric Blue", "Midnight Blue",
            "Ocean Blue", "Pacific Blue", "Eggshell", "Alabaster", "Snow", "Pearl", "Opal",
            "Onyx", "Graphite", "Jet Black", "Neon Green", "Neon Pink", "Neon Yellow",
            "Neon Orange", "Neon Blue", "Neon Purple", "Gold Metallic", "Silver Metallic",
            "Bronze Metallic", "Copper Metallic", "Pearlescent White", "Iridescent Blue",
            "Glitter Silver", "Glitter Gold"
        };
        string[] volumes = {
            "5ml", "8ml", "10ml", "15ml", "20ml", "30ml", "50ml", "60ml", "100ml", "120ml",
            "200ml", "250ml", "300ml", "400ml", "500ml", "750ml", "1L", "2L", "5L",
            "Cartridge", "Refill pack", "Bottle", "Tube", "Drum"
        };

        foreach (var type in types)
            foreach (var color in colors)
                foreach (var volume in volumes)
                    yield return $"{type} {color} {volume}";
    }

    private static IEnumerable<string> GenerateBuildingMaterials()
    {
        string[] items = {
            "Brick", "Cinder block", "Concrete block", "Clay block", "AAC block", "Stone block",
            "Drywall panel", "Gypsum board", "Cement board", "Fiber cement board", "Plasterboard",
            "Plywood sheet", "OSB board", "MDF panel", "Particle board", "Hardboard",
            "Insulation board", "Foam board", "Mineral wool board", "Fiberglass board",
            "Lumber", "Stud", "Joist", "Beam", "Rafter", "Sill plate", "Subfloor panel",
            "Roofing shingle", "Roofing tile", "Roof panel", "Siding panel", "Wall panel",
            "Floor tile", "Wall tile", "Ceiling tile", "Acoustic panel", "Glass pane",
            "Rebar", "Mesh", "Wire lath", "Vapor barrier", "House wrap", "Underlayment",
            "Flashing", "Gutter", "Downspout", "Corner bead", "Expansion joint", "Sealant",
            "Mortar", "Grout", "Adhesive", "Caulk"
        };
        string[] materials = {
            "Clay", "Concrete", "Cement", "Gypsum", "Lime", "Sandstone", "Granite", "Marble",
            "Slate", "Pine", "Spruce", "Fir", "Cedar", "Oak", "Maple", "Bamboo",
            "Engineered wood", "Steel", "Galvanized steel", "Stainless steel", "Aluminum",
            "Copper", "PVC", "Vinyl", "Fiberglass", "Foam", "Mineral wool", "Glass",
            "Ceramic", "Porcelain", "Bitumen", "Asphalt", "Rubber"
        };
        string[] sizes = {
            "8x4x2 in", "16x8x8 in", "12x8x8 in", "24x8x8 in", "4x8 ft", "4x10 ft", "4x12 ft",
            "2x4 ft", "2x2 ft", "1/2 in thick", "5/8 in thick", "3/4 in thick", "1 in thick",
            "2x4 in", "2x6 in", "2x8 in", "2x10 in", "2x12 in", "4x4 in", "6x6 in", "8x8 in",
            "10x10 in", "12x12 in", "18x18 in", "24x24 in", "6x24 in", "6x36 in", "8x48 in",
            "36x12 in", "36x36 in", "48x16 in", "#3 (3/8 in)", "#4 (1/2 in)", "#5 (5/8 in)",
            "6x6 in mesh", "100 ft roll", "50 ft roll", "25 ft roll", "10 ft length"
        };

        foreach (var item in items)
            foreach (var material in materials)
                foreach (var size in sizes)
                    yield return $"{item} {material} {size}";
    }

    private static IEnumerable<string> GenerateAutomotiveParts()
    {
        string[] items = {
            "Air filter", "Oil filter", "Fuel filter", "Cabin filter", "Spark plug", "Ignition coil",
            "Alternator", "Starter motor", "Battery", "Radiator", "Water pump", "Thermostat",
            "Timing belt", "Serpentine belt", "Brake pad", "Brake rotor", "Brake caliper",
            "Brake drum", "Brake shoe", "Wheel bearing", "Hub assembly", "Axle shaft", "CV joint",
            "Drive shaft", "Shock absorber", "Strut", "Control arm", "Ball joint", "Tie rod end",
            "Steering rack", "Power steering pump", "Fuel pump", "Fuel injector", "Oxygen sensor",
            "MAP sensor", "MAF sensor", "Throttle body", "Exhaust manifold", "Catalytic converter",
            "Muffler", "Tailpipe", "Headlight", "Taillight", "Turn signal", "Fog light",
            "Wiper blade", "Door handle", "Window regulator", "Mirror", "Grille", "Bumper"
        };
        string[] materials = {
            "Steel", "Aluminum", "Cast iron", "Plastic", "Rubber", "Copper", "Brass", "Ceramic",
            "Glass", "Composite"
        };
        string[] sizes = {
            "Standard", "Heavy duty", "Compact", "Extended", "High performance", "12V", "24V",
            "5mm", "10mm", "15mm", "20mm", "1/2 in", "3/4 in", "1 in", "Small", "Medium", "Large"
        };
        
        foreach (var item in items)
            foreach (var material in materials)
                foreach (var size in sizes)
                    yield return $"{item} {material} {size}";
    }

    private static IEnumerable<string> GenerateTopWear()
    {
        string[] items = {
            "T-shirt", "Polo shirt", "Dress shirt", "Button-down shirt", "Blouse", "Tank top",
            "Camisole", "Crop top", "Long-sleeve shirt", "Short-sleeve shirt", "Henley shirt",
            "Baseball shirt", "Rugby shirt", "Sweatshirt", "Hoodie", "Pullover", "Cardigan",
            "Sweater", "V-neck sweater", "Crewneck sweater", "Turtleneck", "Mock neck", "Poncho",
            "Kimono top", "Peplum top", "Wrap top", "Tube top", "Bodysuit", "Tunic",
            "Peasant top", "Jacket", "Bomber jacket", "Denim jacket", "Leather jacket", "Blazer",
            "Suit jacket", "Windbreaker", "Track jacket", "Varsity jacket", "Parka", "Rain jacket",
            "Anorak", "Fleece jacket", "Quilted jacket", "Down jacket", "Vest", "Gilet", "Shrug",
            "Cape", "Bolero"
        };
        string[] materials = {
            "Cotton", "Linen", "Polyester", "Rayon", "Silk", "Wool", "Cashmere", "Acrylic",
            "Nylon", "Spandex", "Modal", "Viscose", "Bamboo", "Denim", "Leather",
            "Faux leather", "Fleece", "Velvet", "Jersey", "Satin", "Chiffon", "Lace", "Tweed",
            "Corduroy", "Suede", "Mesh", "Microfiber", "Canvas", "Terrycloth", "Hemp"
        };
        string[] colors = {
            "Black", "White", "Gray", "Charcoal", "Navy", "Blue", "Light blue", "Brown", "Tan",
            "Beige", "Cream", "Red", "Burgundy", "Maroon", "Pink", "Purple", "Lavender",
            "Green", "Olive", "Dark green", "Yellow", "Mustard", "Orange", "Coral", "Gold",
            "Silver", "Teal", "Turquoise", "Mint", "Ivory", "Copper"
        };
        string[] sizes = {
            "XS", "S", "M", "L", "XL", "XXL", "3XL", "4XL", "5XL", "US 0", "US 2", "US 4",
            "US 6", "US 8", "US 10", "US 12", "US 14", "US 16", "US 18", "US 20", "EU 32",
            "EU 34", "EU 36", "EU 38", "EU 40", "EU 42", "EU 44", "EU 46", "EU 48", "EU 50"
        };

        foreach (var item in items)
            foreach (var material in materials)
                foreach (var color in colors)
                    foreach (var size in sizes)
                        yield return $"{item} {material} {color} {size}";
    }

    private static IEnumerable<string> GenerateShoes()
    {
        string[] items = {
            "Sneaker", "Running shoe", "Dress shoe", "Loafer", "Oxford", "Derby", "Boot",
            "Chelsea boot", "Chukka boot", "Hiking boot", "Sandals", "Flip-flop", "Slipper",
            "Moccasin", "Espadrille", "Ballet flat", "Pump", "Wedge", "Clog", "Monk strap",
            "High heel", "Platform shoe", "Skate shoe", "Soccer cleat", "Basketball shoe",
            "Tennis shoe", "Golf shoe", "Work boot", "Rain boot", "Snow boot"
        };
        string[] materials = {
            "Leather", "Synthetic leather", "Canvas", "Mesh", "Suede", "Rubber", "Textile",
            "Nubuck", "Patent leather", "Denim", "PVC", "PU", "EVA", "Foam", "Wool", "Cotton",
            "Polyester", "Microfiber", "Faux fur", "Neoprene"
        };
        string[] colors = {
            "Black", "White", "Gray", "Charcoal", "Navy", "Blue", "Light blue", "Brown", "Tan",
            "Beige", "Cream", "Red", "Burgundy", "Maroon", "Pink", "Purple", "Lavender",
            "Green", "Olive", "Dark green", "Yellow", "Mustard", "Orange", "Coral", "Gold",
            "Silver", "Teal", "Turquoise", "Mint", "Ivory", "Copper"
        };
        string[] sizes = {
            "US 5", "US 6", "US 7", "US 8", "US 9", "US 10", "US 11", "US 12", "US 13",
            "EU 36", "EU 37", "EU 38", "EU 39", "EU 40", "EU 41", "EU 42", "EU 43", "EU 44",
            "EU 45", "UK 3", "UK 4", "UK 5", "UK 6", "UK 7", "UK 8", "UK 9", "UK 10",
            "UK 11", "Small", "Medium", "Large", "Extra large"
        };

        foreach (var item in items)
            foreach (var material in materials)
                foreach (var color in colors)
                    foreach (var size in sizes)
                        yield return $"{item} {material} {color} {size}";
    }

    private static IEnumerable<string> GeneratePants()
    {
        string[] items = {
            "Jeans", "Chinos", "Dress pants", "Cargo pants", "Joggers", "Sweatpants",
            "Track pants", "Corduroy pants", "Khakis", "Work pants", "Overalls", "Shorts",
            "Capri pants", "Cropped pants", "Palazzo pants", "Culottes", "Leggings", "Trousers",
            "Slim pants", "Wide-leg pants", "Bootcut pants", "Straight-leg pants", "Pleated pants",
            "Flat-front pants", "High-waisted pants", "Low-rise pants", "Paperbag pants",
            "Harem pants", "Carpenter pants", "Painter pants"
        };
        string[] materials = {
            "Denim", "Cotton", "Polyester", "Linen", "Wool", "Corduroy", "Twill", "Rayon",
            "Spandex", "Nylon", "Leather", "Faux leather", "Viscose", "Modal", "Silk",
            "Canvas", "Fleece", "Velvet", "Jersey", "Microfiber"
        };
        string[] fits = {
            "Slim fit", "Regular fit", "Relaxed fit", "Loose fit", "Skinny fit", "Tapered fit",
            "Straight fit", "Bootcut fit", "Wide fit", "Athletic fit"
        };
        string[] colors = {
            "Black", "Navy", "Charcoal", "Gray", "Light gray", "White", "Beige", "Khaki",
            "Brown", "Olive", "Green", "Dark green", "Blue", "Light blue", "Red", "Burgundy",
            "Maroon", "Yellow", "Mustard", "Tan", "Cream", "Purple", "Lavender", "Pink",
            "Coral", "Orange", "Rust", "Teal", "Turquoise", "Mint", "Gold", "Silver"
        };
        string[] sizes = {
            "28x30", "28x32", "30x30", "30x32", "30x34", "32x30", "32x32", "32x34", "34x30",
            "34x32", "34x34", "36x30", "36x32", "36x34", "38x30", "38x32", "38x34", "S",
            "M", "L", "XL", "XXL", "3XL", "4XL", "XS", "XXS", "5XL"
        };

        foreach (var item in items)
            foreach (var material in materials)
                foreach (var fit in fits)
                    foreach (var color in colors)
                        foreach (var size in sizes)
                            yield return $"{item} {material} {fit} {color} {size}";
    }

    private static IEnumerable<string> GenerateGemstones()
    {
        string[] gemstones = {
            "Diamond", "Ruby", "Sapphire", "Emerald", "Amethyst", "Topaz", "Aquamarine",
            "Garnet", "Opal", "Peridot", "Citrine", "Spinel", "Tanzanite", "Tourmaline",
            "Alexandrite", "Morganite", "Jade", "Onyx", "Zircon", "Quartz", "Agate",
            "Lapis Lazuli", "Moonstone", "Sunstone", "Turquoise", "Malachite", "Chalcedony",
            "Bloodstone", "Jasper", "Obsidian", "Amber", "Coral", "Pearl", "Hematite",
            "Labradorite", "Rhodolite", "Kunzite", "Iolite", "Sodalite", "Amazonite",
            "Apatite", "Carnelian", "Chrysoprase", "Fluorite", "Howlite", "Larimar",
            "Prehnite", "Serpentine", "Smithsonite", "Sugilite"
        };
        string[] cuts = {
            "Round", "Oval", "Cushion", "Princess", "Emerald", "Asscher", "Marquise", "Pear",
            "Heart", "Radiant", "Trillion", "Baguette", "Cabochon", "Briolette", "Rose cut",
            "Old mine cut", "Step cut", "Mixed cut", "Fancy cut"
        };
        string[] colors = {
            "Colorless", "White", "Black", "Gray", "Brown", "Yellow", "Orange", "Red", "Pink",
            "Purple", "Violet", "Blue", "Green", "Teal", "Turquoise", "Indigo", "Champagne",
            "Cognac", "Peach", "Lavender", "Lilac", "Magenta", "Fuchsia", "Olive", "Lime",
            "Mint", "Aqua", "Sky blue", "Royal blue", "Navy", "Emerald green", "Forest green"
        };
        string[] sizes = {
            "1mm", "2mm", "3mm", "4mm", "5mm", "6mm", "7mm", "8mm", "9mm", "10mm", "12mm",
            "14mm", "16mm", "18mm", "20mm", "0.25ct", "0.5ct", "0.75ct", "1ct", "1.5ct",
            "2ct", "3ct", "5ct", "10ct"
        };

        foreach (var gemstone in gemstones)
            foreach (var cut in cuts)
                foreach (var color in colors)
                    foreach (var size in sizes)
                        yield return $"{gemstone} {cut} {color} {size}";
    }
}