# Delphi_Csharp_NiDAQmx
Embarcadero Delphi and NiDAQmx – the solution of the problem

The absence of official support of interaction between NiDAQmx driver
and RAD Borland (Embarcadero) Delphi is the actual problem. Many
application developers want to support existing projects and develop new
ones using the familiar programming environment. However, we suppose
that new-created Delphi projects must support various DAQ cards and
operation systems of the Windows family including 64-bit versions.
Existing Delphi applications written with the legacy Traditional NiDAQ
driver must be easy converted to work with a new NiDAQmx driver.

There are two methods to solve this problem partially.

1. Using TDAQ7.4 for Windows Vista. This method allows to use existing
Delphi applications for Windows Vista x86 and Windows 7 x86. But this
driver doesn’t support a lot of modern DAQ cards and doesn’t work in
64-bit operations systems. Using TDAQ7.4 driver for Windows Vista can be
only a temporary solution.

2. Using wrappers for the nidaqmx.dll library. The limitations of this
method are dependence between NiDAQmx version and wrapper version
(changing of driver’s version involves changing wrapper); nidaqmx.dll
functions are not well documented; there are strange bugs and memory
leaks; 64-bit operation systems are not supported yet; working of this
method is not guaranteed at all.

I want to propose a new solution of interaction of the NiDAQmx driver
and Borland Delphi. The essence of the approach is described below.

1. Create .NET-assembly using Microsoft Visual C# 2008 Express Edition
(for example), in which describe one or several objects  contained
necessary DAQ-functionality.
2. Select names of methods, properties and events corresponding with
name of old driver’s members. This creates an opportunity to simplify
the process of remaking existing Delphi applications.
3. Register the assembly for Com Interop.
4. Import the .NET-assembly in Borland Delphi. Now we have an object the
same as, for example, ActiveX object with an encapsulated necessary
DAQ-functionality.

Advantages of described approach are:
Working with all DAQ-cards in all operation systems including 64-bit
ones.
Independence from NiDAQmx versions.
You need only free Microsoft Visual C# 2008 Express Edition (for
example), you don’t need any expensive software.
An opportunity to use Delphi as basic software for DAQ application
development.

Using described method I have already created .NET assembly with
realization several functions for one-shot digital reading and writing,
continuous analog reading, one-shot analog writing (I need only this
functionality for my own applications).

If this topic is interesting for you please write me karpesh@mail.ru or
soft@electrolab.ru (English and Russian available).
