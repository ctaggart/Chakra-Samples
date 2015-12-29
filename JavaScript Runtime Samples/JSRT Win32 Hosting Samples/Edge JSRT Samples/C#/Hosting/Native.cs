﻿namespace Taser.Chakra
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Native interfaces.
    /// </summary>
    public static class Native
    {
        /// <summary>
        ///     Event mask for profiling.
        /// </summary>
        public enum ProfilerEventMask
        {
            /// <summary>
            ///     Trace calls to script functions.
            /// </summary>
            TraceScriptFunctionCall = 0x1,

            /// <summary>
            ///     Trace calls to built-in functions.
            /// </summary>
            TraceNativeFunctionCall = 0x2,

            /// <summary>
            ///     Trace calls to DOM methods.
            /// </summary>
            TraceDomFunctionCall = 0x4,

            /// <summary>
            ///     Trace all calls except DOM methods.
            /// </summary>
            TraceAll = (TraceScriptFunctionCall | TraceNativeFunctionCall),

            /// <summary>
            ///     Trace all calls.
            /// </summary>
            TraceAllWithDom = (TraceAll | TraceDomFunctionCall)
        }

        /// <summary>
        ///     Profiled script type.
        /// </summary>
        public enum ProfilerScriptType
        {
            /// <summary>
            ///     A user script.
            /// </summary>
            User,

            /// <summary>
            ///     A dynamic script.
            /// </summary>
            Dynamic,

            /// <summary>
            ///     A native script.
            /// </summary>
            Native,

            /// <summary>
            ///     A DOM-related script.
            /// </summary>
            Dom
        }

        /// <summary>
        ///     IActiveScriptProfilerCallback COM interface.
        /// </summary>
        [Guid("740eca23-7d9d-42e5-ba9d-f8b24b1c7a9b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IActiveScriptProfilerCallback
        {
            /// <summary>
            ///     Called when the profile is started.
            /// </summary>
            /// <param name="context">The context provided when profiling was started.</param>
            void Initialize(uint context);

            /// <summary>
            ///     Called when profiling is stopped.
            /// </summary>
            /// <param name="reason">The reason code provided when profiling was stopped.</param>
            void Shutdown(uint reason);

            /// <summary>
            ///     Called when a script is compiled.
            /// </summary>
            /// <param name="scriptId">The ID of the script.</param>
            /// <param name="type">The type of the script.</param>
            /// <param name="debugDocumentContext">The debug document context, if any.</param>
            void ScriptCompiled(int scriptId, ProfilerScriptType type, IntPtr debugDocumentContext);

            /// <summary>
            ///     Called when a function is compiled.
            /// </summary>
            /// <param name="functionId">The ID of the function.</param>
            /// <param name="scriptId">The ID of the script.</param>
            /// <param name="functionName">The name of the function.</param>
            /// <param name="functionNameHint">The function name hint.</param>
            /// <param name="debugDocumentContext">The debug document context, if any.</param>
            void FunctionCompiled(int functionId, int scriptId, [MarshalAs(UnmanagedType.LPWStr)] string functionName, [MarshalAs(UnmanagedType.LPWStr)] string functionNameHint, IntPtr debugDocumentContext);

            /// <summary>
            ///     Called when a function is entered.
            /// </summary>
            /// <param name="scriptId">The ID of the script.</param>
            /// <param name="functionId">The ID of the function.</param>
            void OnFunctionEnter(int scriptId, int functionId);

            /// <summary>
            ///     Called when a function is exited.
            /// </summary>
            /// <param name="scriptId">The ID of the script.</param>
            /// <param name="functionId">The ID of the function.</param>
            void OnFunctionExit(int scriptId, int functionId);
        }

        /// <summary>
        ///     IActiveScriptProfilerCallback2 COM interface.
        /// </summary>
        [Guid("31B7F8AD-A637-409C-B22F-040995B6103D")]
        public interface IActiveScriptProfilerCallback2 : IActiveScriptProfilerCallback
        {
            /// <summary>
            ///     Called when a function is entered by name.
            /// </summary>
            /// <param name="functionName">The name of the function.</param>
            /// <param name="type">The type of the function.</param>
            void OnFunctionEnterByName(string functionName, ProfilerScriptType type);

            /// <summary>
            ///     Called when a function is exited by name.
            /// </summary>
            /// <param name="functionName">The name of the function.</param>
            /// <param name="type">The type of the function.</param>
            void OnFunctionExitByName(string functionName, ProfilerScriptType type);
        }

        /// <summary>
        ///     IActiveScriptProfilerHeapEnum COM interface.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Name defined in COM.")]
        [Guid("32E4694E-0D37-419B-B93D-FA20DED6E8EA")]
        public interface IActiveScriptProfilerHeapEnum
        {
        }

        /// <summary>
        /// Throws if a native method returns an error code.
        /// </summary>
        /// <param name="error">The error.</param>
        public static void ThrowIfError(JavaScriptErrorCode error)
        {
            if (error != JavaScriptErrorCode.NoError)
            {
                switch (error)
                {
                    case JavaScriptErrorCode.InvalidArgument:
                        throw new JavaScriptUsageException(error, "Invalid argument.");

                    case JavaScriptErrorCode.NullArgument:
                        throw new JavaScriptUsageException(error, "Null argument.");

                    case JavaScriptErrorCode.NoCurrentContext:
                        throw new JavaScriptUsageException(error, "No current context.");

                    case JavaScriptErrorCode.InExceptionState:
                        throw new JavaScriptUsageException(error, "Runtime is in exception state.");

                    case JavaScriptErrorCode.NotImplemented:
                        throw new JavaScriptUsageException(error, "Method is not implemented.");

                    case JavaScriptErrorCode.WrongThread:
                        throw new JavaScriptUsageException(error, "Runtime is active on another thread.");

                    case JavaScriptErrorCode.RuntimeInUse:
                        throw new JavaScriptUsageException(error, "Runtime is in use.");

                    case JavaScriptErrorCode.BadSerializedScript:
                        throw new JavaScriptUsageException(error, "Bad serialized script.");

                    case JavaScriptErrorCode.InDisabledState:
                        throw new JavaScriptUsageException(error, "Runtime is disabled.");

                    case JavaScriptErrorCode.CannotDisableExecution:
                        throw new JavaScriptUsageException(error, "Cannot disable execution.");

                    case JavaScriptErrorCode.AlreadyDebuggingContext:
                        throw new JavaScriptUsageException(error, "Context is already in debug mode.");

                    case JavaScriptErrorCode.HeapEnumInProgress:
                        throw new JavaScriptUsageException(error, "Heap enumeration is in progress.");

                    case JavaScriptErrorCode.ArgumentNotObject:
                        throw new JavaScriptUsageException(error, "Argument is not an object.");

                    case JavaScriptErrorCode.InProfileCallback:
                        throw new JavaScriptUsageException(error, "In a profile callback.");

                    case JavaScriptErrorCode.InThreadServiceCallback:
                        throw new JavaScriptUsageException(error, "In a thread service callback.");

                    case JavaScriptErrorCode.CannotSerializeDebugScript:
                        throw new JavaScriptUsageException(error, "Cannot serialize a debug script.");

                    case JavaScriptErrorCode.AlreadyProfilingContext:
                        throw new JavaScriptUsageException(error, "Already profiling this context.");

                    case JavaScriptErrorCode.IdleNotEnabled:
                        throw new JavaScriptUsageException(error, "Idle is not enabled.");

                    case JavaScriptErrorCode.OutOfMemory:
                        throw new JavaScriptEngineException(error, "Out of memory.");

                    case JavaScriptErrorCode.ScriptException:
                        {
                            JavaScriptValue errorObject;
                            JavaScriptErrorCode innerError = JsGetAndClearException(out errorObject);

                            if (innerError != JavaScriptErrorCode.NoError)
                            {
                                throw new JavaScriptFatalException(innerError);
                            }

                            throw new JavaScriptScriptException(error, errorObject, "Script threw an exception.");
                        }

                    case JavaScriptErrorCode.ScriptCompile:
                        {
                            JavaScriptValue errorObject;
                            JavaScriptErrorCode innerError = JsGetAndClearException(out errorObject);

                            if (innerError != JavaScriptErrorCode.NoError)
                            {
                                throw new JavaScriptFatalException(innerError);
                            }

                            throw new JavaScriptScriptException(error, errorObject, "Compile error.");
                        }

                    case JavaScriptErrorCode.ScriptTerminated:
                        throw new JavaScriptScriptException(error, JavaScriptValue.Invalid, "Script was terminated.");

                    case JavaScriptErrorCode.ScriptEvalDisabled:
                        throw new JavaScriptScriptException(error, JavaScriptValue.Invalid, "Eval of strings is disabled in this runtime.");

                    case JavaScriptErrorCode.Fatal:
                        throw new JavaScriptFatalException(error);

                    default:
                        throw new JavaScriptFatalException(error);
                }
            }
        }

        [DllImport("Chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateRuntime(JavaScriptRuntimeAttributes attributes, JavaScriptThreadServiceCallback threadService, out JavaScriptRuntime runtime);

        [DllImport("Chakra.dll")]
        public static extern JavaScriptErrorCode JsCollectGarbage(JavaScriptRuntime handle);

        [DllImport("Chakra.dll")]
        public static extern JavaScriptErrorCode JsDisposeRuntime(JavaScriptRuntime handle);

        [DllImport("Chakra.dll")]
        public static extern JavaScriptErrorCode JsGetRuntimeMemoryUsage(JavaScriptRuntime runtime, out UIntPtr memoryUsage);

        [DllImport("Chakra.dll")]
        public static extern JavaScriptErrorCode JsGetRuntimeMemoryLimit(JavaScriptRuntime runtime, out UIntPtr memoryLimit);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetRuntimeMemoryLimit(JavaScriptRuntime runtime, UIntPtr memoryLimit);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetRuntimeMemoryAllocationCallback(JavaScriptRuntime runtime, IntPtr callbackState, JavaScriptMemoryAllocationCallback allocationCallback);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetRuntimeBeforeCollectCallback(JavaScriptRuntime runtime, IntPtr callbackState, JavaScriptBeforeCollectCallback beforeCollectCallback);

        [DllImport("chakra.dll", EntryPoint = "JsAddRef")]
        public static extern JavaScriptErrorCode JsContextAddRef(JavaScriptContext reference, out uint count);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsAddRef(JavaScriptValue reference, out uint count);

        [DllImport("chakra.dll", EntryPoint = "JsRelease")]
        public static extern JavaScriptErrorCode JsContextRelease(JavaScriptContext reference, out uint count);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsRelease(JavaScriptValue reference, out uint count);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateContext(JavaScriptRuntime runtime, out JavaScriptContext newContext);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetCurrentContext(out JavaScriptContext currentContext);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetCurrentContext(JavaScriptContext context);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetRuntime(JavaScriptContext context, out JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsStartDebugging();

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsIdle(out uint nextIdleTick);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsParseScript(string script, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsRunScript(string script, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSerializeScript(string script, byte[] buffer, ref ulong bufferSize);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsParseSerializedScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsRunSerializedScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceUrl, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetPropertyIdFromName(string name, out JavaScriptPropertyId propertyId);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetPropertyNameFromId(JavaScriptPropertyId propertyId, out string name);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetUndefinedValue(out JavaScriptValue undefinedValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetNullValue(out JavaScriptValue nullValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetTrueValue(out JavaScriptValue trueValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetFalseValue(out JavaScriptValue falseValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsBoolToBoolean(bool value, out JavaScriptValue booleanValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsBooleanToBool(JavaScriptValue booleanValue, out bool boolValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsConvertValueToBoolean(JavaScriptValue value, out JavaScriptValue booleanValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetValueType(JavaScriptValue value, out JavaScriptValueType type);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsDoubleToNumber(double doubleValue, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsIntToNumber(int intValue, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsNumberToDouble(JavaScriptValue value, out double doubleValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsConvertValueToNumber(JavaScriptValue value, out JavaScriptValue numberValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetStringLength(JavaScriptValue sringValue, out int length);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsPointerToString(string value, UIntPtr stringLength, out JavaScriptValue stringValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsStringToPointer(JavaScriptValue value, out IntPtr stringValue, out UIntPtr stringLength);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsConvertValueToString(JavaScriptValue value, out JavaScriptValue stringValue);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsVariantToValue([MarshalAs(UnmanagedType.Struct)] ref object var, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsValueToVariant(JavaScriptValue obj, [MarshalAs(UnmanagedType.Struct)] out object var);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetGlobalObject(out JavaScriptValue globalObject);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateObject(out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateExternalObject(IntPtr data, JavaScriptObjectFinalizeCallback finalizeCallback, out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsConvertValueToObject(JavaScriptValue value, out JavaScriptValue obj);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetPrototype(JavaScriptValue obj, out JavaScriptValue prototypeObject);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetPrototype(JavaScriptValue obj, JavaScriptValue prototypeObject);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetExtensionAllowed(JavaScriptValue obj, out bool value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsPreventExtension(JavaScriptValue obj);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, out JavaScriptValue value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetOwnPropertyDescriptor(JavaScriptValue obj, JavaScriptPropertyId propertyId, out JavaScriptValue propertyDescriptor);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetOwnPropertyNames(JavaScriptValue obj, out JavaScriptValue propertyNames);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, JavaScriptValue value, bool useStrictRules);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsHasProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, out bool hasProperty);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsDeleteProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, bool useStrictRules, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsDefineProperty(JavaScriptValue obj, JavaScriptPropertyId propertyId, JavaScriptValue propertyDescriptor, out bool result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsHasIndexedProperty(JavaScriptValue obj, JavaScriptValue index, out bool result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetIndexedProperty(JavaScriptValue obj, JavaScriptValue index, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetIndexedProperty(JavaScriptValue obj, JavaScriptValue index, JavaScriptValue value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsDeleteIndexedProperty(JavaScriptValue obj, JavaScriptValue index);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsEquals(JavaScriptValue obj1, JavaScriptValue obj2, out bool result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsStrictEquals(JavaScriptValue obj1, JavaScriptValue obj2, out bool result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsHasExternalData(JavaScriptValue obj, out bool value);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetExternalData(JavaScriptValue obj, out IntPtr externalData);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetExternalData(JavaScriptValue obj, IntPtr externalData);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateArray(uint length, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCallFunction(JavaScriptValue function, JavaScriptValue[] arguments, ushort argumentCount, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsConstructObject(JavaScriptValue function, JavaScriptValue[] arguments, ushort argumentCount, out JavaScriptValue result);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateFunction(JavaScriptNativeFunction nativeFunction, IntPtr externalData, out JavaScriptValue function);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateRangeError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateReferenceError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateSyntaxError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateTypeError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsCreateURIError(JavaScriptValue message, out JavaScriptValue error);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsHasException(out bool hasException);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsGetAndClearException(out JavaScriptValue exception);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsSetException(JavaScriptValue exception);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsDisableRuntimeExecution(JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsEnableRuntimeExecution(JavaScriptRuntime runtime);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsIsRuntimeExecutionDisabled(JavaScriptRuntime runtime, out bool isDisabled);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsStartProfiling(IActiveScriptProfilerCallback callback, ProfilerEventMask eventMask, int context);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsStopProfiling(int reason);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsEnumerateHeap(out IActiveScriptProfilerHeapEnum enumerator);

        [DllImport("chakra.dll")]
        public static extern JavaScriptErrorCode JsIsEnumeratingHeap(out bool isEnumeratingHeap);
    }
}
