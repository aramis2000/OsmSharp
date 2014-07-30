﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2013 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using OsmSharp.WinForms.UI.IO.MemoryMappedFiles;
using OsmSharp.WinForms.UI.Renderer.Images;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace OsmSharp.WinForms.UI
{
    /// <summary>
    /// Class responsable for creating native hooks for platform-specific functionality.
    /// </summary>
    public static class Native
    {
        /// <summary>
        /// Initializes some platform-specifics for OsmSharp to use.
        /// </summary>
        public static void Initialize()
        {
            // intialize the native image cache factory.
            OsmSharp.UI.Renderer.Images.NativeImageCacheFactory.SetDelegate(
                () =>
                {
                    return new NativeImageCache();
                });
            OsmSharp.IO.MemoryMappedFiles.NativeMemoryMappedFileFactory.SetDelegates(
                (path) =>
                {
                    var file = new FileInfo(path);
                    if (!file.Exists)
                    { // make sure to create the file if it does not exist yet!
                        var fileStream = file.Create();
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                    return new MemoryMappedFileWrapper(MemoryMappedFile.CreateFromFile(path, FileMode.OpenOrCreate, file.Name, 1000, MemoryMappedFileAccess.ReadWrite));
                },
                (mapName, capacity) =>
                {
                    return new MemoryMappedFileWrapper(MemoryMappedFile.CreateNew(mapName, capacity));
                });
        }
    }
}