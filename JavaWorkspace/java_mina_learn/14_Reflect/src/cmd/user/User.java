// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: message/user.proto

package cmd.user;

public final class User {
  private User() {}
  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistry registry) {
  }
  static com.google.protobuf.Descriptors.Descriptor
    internal_static_cmd_user_Login_descriptor;
  static
    com.google.protobuf.GeneratedMessage.FieldAccessorTable
      internal_static_cmd_user_Login_fieldAccessorTable;
  static com.google.protobuf.Descriptors.Descriptor
    internal_static_cmd_user_LoginResult_descriptor;
  static
    com.google.protobuf.GeneratedMessage.FieldAccessorTable
      internal_static_cmd_user_LoginResult_fieldAccessorTable;

  public static com.google.protobuf.Descriptors.FileDescriptor
      getDescriptor() {
    return descriptor;
  }
  private static com.google.protobuf.Descriptors.FileDescriptor
      descriptor;
  static {
    java.lang.String[] descriptorData = {
      "\n\022message/user.proto\022\010cmd.user\"+\n\005Login\022" +
      "\020\n\010username\030\001 \002(\t\022\020\n\010password\030\002 \002(\t\"\035\n\013L" +
      "oginResult\022\016\n\006result\030\001 \002(\005B\002P\001"
    };
    com.google.protobuf.Descriptors.FileDescriptor.InternalDescriptorAssigner assigner =
      new com.google.protobuf.Descriptors.FileDescriptor.InternalDescriptorAssigner() {
        public com.google.protobuf.ExtensionRegistry assignDescriptors(
            com.google.protobuf.Descriptors.FileDescriptor root) {
          descriptor = root;
          internal_static_cmd_user_Login_descriptor =
            getDescriptor().getMessageTypes().get(0);
          internal_static_cmd_user_Login_fieldAccessorTable = new
            com.google.protobuf.GeneratedMessage.FieldAccessorTable(
              internal_static_cmd_user_Login_descriptor,
              new java.lang.String[] { "Username", "Password", });
          internal_static_cmd_user_LoginResult_descriptor =
            getDescriptor().getMessageTypes().get(1);
          internal_static_cmd_user_LoginResult_fieldAccessorTable = new
            com.google.protobuf.GeneratedMessage.FieldAccessorTable(
              internal_static_cmd_user_LoginResult_descriptor,
              new java.lang.String[] { "Result", });
          return null;
        }
      };
    com.google.protobuf.Descriptors.FileDescriptor
      .internalBuildGeneratedFileFrom(descriptorData,
        new com.google.protobuf.Descriptors.FileDescriptor[] {
        }, assigner);
  }

  // @@protoc_insertion_point(outer_class_scope)
}
