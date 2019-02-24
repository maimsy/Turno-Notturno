
colors = ["Blue", "Green", "Red"]
themes = ["Abstract", "Portrait", "Cityscape"]
materials = ["Wood", "Bronze", "Cityscape"]
quirks = ["Technology", "Spirals", "Teeth"]
properties = [colors, themes, materials, quirks]

paintings = [(0, 2, 0, 1),
			 (2, 1, 1, 1),
			 (1, 0, 1, 2),
			 (0, 0, 0, 0),
			 (0, 2, 1, 2),
			 (2, 0, 0, 1)]
	
print("Paintings:")
for painting in paintings:
	s = ""
	for i in range(len(painting)):
		s += properties[i][painting[i]] + "\t\t"
	print(s)


print("")
print("All solutions:")
for color in range(3):
    for theme in range(3):
        for material in range(3):
            for quirk in range(3):
                success = True

                for painting in paintings:
                    correct = 0
                    if (painting[0] == color):
                        correct += 1
                    if (painting[1] == theme):
                        correct += 1
                    if (painting[2] == material):
                        correct += 1
                    if (painting[3] == quirk):
                        correct += 1
                    if correct != 2:
                        success = False
                if (success == True):
                    print(colors[color], themes[theme], materials[material], quirks[quirk], sep="\t\t")
