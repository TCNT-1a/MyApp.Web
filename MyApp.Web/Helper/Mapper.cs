namespace MyApp.Web.Helper
{
    public static class Mapper
    {
        public class  PropertyCoppier<TParent,TChild> where TParent : class  
                                                      where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();
                foreach (var parentProperty in parentProperties)
                {
                    if (parentProperty.Name.ToLower() == "id")
                        continue;
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name.ToLower() == childProperty.Name.ToLower()
                            && parentProperty.PropertyType == childProperty.PropertyType 
                            && childProperty.SetMethod !=null
                            )
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }
    }
        
}
