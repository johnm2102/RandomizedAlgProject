all:
	fsharpc --checked- hashFunctions.fs streamGenerator.fs Program.fs
clean:
	rm *.exe *.dll