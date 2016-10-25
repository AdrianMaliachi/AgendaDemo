using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace AgendaDemo
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MobileServiceClient client;
            IMobileServiceTable<AgendaTabla> tabla;
            client = new MobileServiceClient(Conexion.conexion);
            tabla = client.GetTable<AgendaTabla>();
            Label titulo = new Label()
            {
                Text = "Insertar datos:"
            };
            Entry nombre1 = new Entry();
            nombre1.Placeholder = "Introduce el nombre";
            nombre1.HorizontalTextAlignment = TextAlignment.Center;
            Entry apellido1 = new Entry();
            apellido1.Placeholder = "Introduce el Apellido";
            apellido1.HorizontalTextAlignment = TextAlignment.Center;
            Entry telefono1 = new Entry();
            telefono1.Placeholder = "Introduce numero telefoico";
            telefono1.HorizontalTextAlignment = TextAlignment.Center;

            Label label_buscar = new Label()
            {
                Text = "Introduce el numero telefonico:"
            };
            Entry buscar = new Entry();
            buscar.Placeholder = "Introduce el numero telefonico";
            Button enviar = new Button()
            {
                Text = "Guardar contacto"
            };
            Button leer = new Button()
            {
                Text = "Mostrar tabla de contactos"
            };
            Button busca = new Button()
            {
                Text = "Buscar contacto"
            };

            Button limpiar = new Button()
            {
                Text = "Limpiar campos"
            };
            ListView lista = new ListView();
            ListView lista2 = new ListView();
            ListView lista3 = new ListView();

            limpiar.Clicked += (sender, args) => {
                nombre1.Text = null;
                apellido1.Text = null;
                telefono1.Text = null;
                buscar.Text = null;
            };

                busca.Clicked += async (sender, args) => {
                if (buscar.Text == null )
                {
                    await MainPage.DisplayAlert("Alerta", "Aún no has introducido un numero telefonico ", "OK");
                }

                else
                {
                    try
                    {
                        IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                        string[] arreglo = new string[items.Count()];
                        string[] arreglo2 = new string[items.Count()];
                        string[] arreglo3 = new string[items.Count()];
                        int i = 0;
                        foreach (var x in items)
                        {
                            arreglo[i] = x.Name;
                            arreglo2[i] = x.Lastname;
                            arreglo3[i] = x.Cellphone;
                            i++;

                            if (x.Cellphone == buscar.Text)
                            {

                                nombre1.Text = x.Name;
                                apellido1.Text = x.Lastname;
                                telefono1.Text = x.Cellphone;
                            }
                        }

                    }
                    catch (Exception e) {
                        await MainPage.DisplayAlert("Alerta", "El contacto que estas intentanto buscar no esta disponoble \n o ha sido eliminado: ", "OK");
                        busca.Text = "";
                    }
                }

            };


            leer.Clicked += async (sender, args) =>
            {
                IEnumerable<AgendaTabla> items = await tabla.ToEnumerableAsync();
                string[] arreglo = new string[items.Count()];
                string[] arreglo2 = new string[items.Count()];
                string[] arreglo3 = new string[items.Count()];
                int i = 0;
                foreach (var x in items)
                {
                    arreglo[i] = x.Name;
                    arreglo2[i] = x.Lastname;
                    arreglo3[i] = x.Cellphone;
                    i++;

                    /* if (x.Cellphone== telefono1.Text) {

                         nombre1.Text = x.Name;
                         apellido1.Text = x.Lastname;
                     }*/
                }


                lista.ItemsSource = arreglo;
                //lista2.ItemsSource = arreglo2;
                lista3.ItemsSource = arreglo3;
            };
            enviar.Clicked += async (sender, args) =>
            {
                if (nombre1.Text == null || telefono1.Text == null || apellido1.Text == null)
                {
                    await MainPage.DisplayAlert("Alerta", "Te falta llenar algun campo ", "OK");
                }

                else
                {

                    var datos = new AgendaTabla { Name = nombre1.Text, Lastname = apellido1.Text, Cellphone = telefono1.Text };

                    await tabla.InsertAsync(datos);
                    await MainPage.DisplayAlert("Alerta", "Tu contacto ha sido guardado con exito ", "OK");
                    nombre1.Text = null;
                    telefono1.Text = null;
                    apellido1.Text = null;
                    IEnumerable<AgendaTabla> items = await tabla
        .ToEnumerableAsync();
                    string[] arreglo = new string[items.Count()];
                    string[] arreglo2 = new string[items.Count()];
                    int i = 0;
                    foreach (var x in items)
                    {
                        arreglo[i] = x.Name;
                        arreglo2[i] = x.Lastname;
                        i++;
                    }

                    lista.ItemsSource = arreglo;
                    lista3.ItemsSource = arreglo2;

                    nombre1.Text = null;
                    telefono1.Text = null;
                    apellido1.Text = null;
                }
                };
                Button actualizar = new Button()
                {
                    Text = "Actualizar Contacto"
                };
                actualizar.Clicked += async (sender, args) =>
                {
                    if (nombre1.Text == null || telefono1.Text == null || apellido1.Text == null)
                    {
                        await MainPage.DisplayAlert("Alerta", "Te falta llenar algun campo para poder actualizar\n el contacto ", "OK");
                    }

                    else
                    {
                        IEnumerable<AgendaTabla> items = await tabla
        .ToEnumerableAsync();
                        string[] arreglo = new string[items.Count()];
                        string[] arreglo2 = new string[items.Count()];
                        string[] ids = new string[items.Count()];
                        string[] arreglo3 = new string[items.Count()];
                        int i = 0;
                        foreach (var x in items)
                        {
                            arreglo[i] = x.Name;
                            arreglo2[i] = x.Lastname;
                            ids[i] = x.Id;
                            arreglo3[i] = x.Cellphone;
                            if (x.Cellphone == telefono1.Text)
                            {
                                if (x.Name != nombre1.Text)
                                {
                                    x.Name = nombre1.Text;
                                }
                                if (x.Lastname != apellido1.Text)
                                {
                                    x.Lastname = apellido1.Text;
                                }
                                await tabla.UpdateAsync(x);
                                await MainPage.DisplayAlert("Alerta", "Tu contacto ha sido actualizado con exito ", "OK");
                                nombre1.Text = null;
                                telefono1.Text = null;
                                apellido1.Text = null;
                            }
                            i++;
                        }
                        lista.ItemsSource = arreglo;
                        lista3.ItemsSource = arreglo3;
                    }
                };

            Button eliminar = new Button();
            eliminar.Text = "Eliminar contacto";
            eliminar.Clicked += async (sender, args) =>
            {
                if (nombre1.Text == null || telefono1.Text == null || apellido1.Text == null)
                {
                    await MainPage.DisplayAlert("Alerta", "Te falta llenar algun campo para poder eliminar\n el contacto ", "OK");
                }

                else
                {
                    IEnumerable<AgendaTabla> items = await tabla
    .ToEnumerableAsync();
                    string[] arreglo = new string[items.Count()];
                    string[] arreglo2 = new string[items.Count()];
                    string[] ids = new string[items.Count()];
                    string[] arreglo3 = new string[items.Count()];
                    int i = 0;
                    foreach (var x in items)
                    {
                        arreglo[i] = x.Name;
                        arreglo2[i] = x.Lastname;
                        ids[i] = x.Id;
                        arreglo3[i] = x.Cellphone;
                        if (x.Cellphone == telefono1.Text)
                        {
                            if (x.Name != nombre1.Text)
                            {
                                x.Name = nombre1.Text;
                            }
                            if (x.Lastname != apellido1.Text)
                            {
                                x.Lastname = apellido1.Text;
                            }
                            await tabla.DeleteAsync(x);
                            await MainPage.DisplayAlert("Alerta", "Tu contacto ha sido eliminado con exito ", "OK");
                            nombre1.Text = null;
                            telefono1.Text = null;
                            apellido1.Text = null;
                        }
                        i++;
                    }
                    lista.ItemsSource = arreglo;
                    lista3.ItemsSource = arreglo3;
                }
            };


            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                   
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }

                }
            };
            grid.Children.Add(lista, 0, 2); ;

            grid.Children.Add(lista3, 1, 2);

            grid.Children.Add(lista2, 2, 3, 1, 3);


            var layout = new StackLayout();
                layout.Children.Add(titulo);
                layout.Children.Add(nombre1);
                layout.Children.Add(apellido1);
                layout.Children.Add(telefono1);
                layout.Children.Add(enviar);
                layout.Children.Add(leer);
                layout.Children.Add(actualizar);
            layout.Children.Add(limpiar);
            layout.Children.Add(eliminar);
            layout.Children.Add(label_buscar);
                layout.Children.Add(buscar);
                layout.Children.Add(busca);
               // layout.Children.Add(lista);
                //layout.Children.Add(lista2);
            layout.Children.Add(grid);
            MainPage = new ContentPage
                {
                    Content = layout
                };
            }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
