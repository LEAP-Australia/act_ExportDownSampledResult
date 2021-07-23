

downsample: builddownsample buildwrapper installlib

buildwrapper:
	cd utility.wrapper && dotnet build -c Release -o ../release

builddownsample: configproj
        cd build && ninja

configproj: builddir
	cd build && cmake -G Ninja -DCMAKE_BUILD_TYPE=Release -DCMAKE_INSTALL_PREFIX=../release ../utility.downsample

install:
installlib:
	cd build && ninja install

.SILENT:
.IGNORE:

builddir: rmbuild
	@mkdir build
rmbuild:
# @rmdir /S /Q build
