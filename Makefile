all:
	fsharpc --checked- streamGenerator.fs hashFunctions.fs hashSumming.fs hashtablechain.fs Program.fs
clean:
	rm *.exe *.dll