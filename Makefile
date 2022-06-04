all:
	fsharpc --checked- hashFunctions.fs streamGenerator.fs hashSumming.fs Program.fs
clean:
	rm *.exe *.dll