all:
	fsharpc --checked- hashFunctions.fs hashSumming.fs hashtablechain.fs streamGenerator.fs Program.fs
clean:
	rm *.exe *.dll