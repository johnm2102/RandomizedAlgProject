all:
	fsharpc --checked- hashFunctions.fs hashSumming.fs hashtablechain.fs squareSumming.fs streamGenerator.fs Program.fs
clean:
	rm *.exe *.dll