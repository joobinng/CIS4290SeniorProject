﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json



Public Class ApiClient
    Inherits System.Web.UI.Page

    Dim httpClient As New HttpClient
    Dim strCartID As String
    Private Async Sub btnAllProducts_ClickAsync(sender As Object, e As EventArgs) Handles btnAllProducts.Click
        Dim uri As String = "https://localhost:44368/api/product"
        Dim task = Await httpClient.GetAsync(uri)
        Dim jsonString = Await task.Content.ReadAsStringAsync()
        If task.IsSuccessStatusCode Then
            Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonString)
            gvAllProducts.DataSource = table
            gvAllProducts.DataBind()
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Private Async Sub btnAllReviews_ClickAsync(sender As Object, e As EventArgs) Handles btnAllReviews.Click
        Dim uri As String = "https://localhost:44368/api/review"
        Dim task = Await httpClient.GetAsync(uri)
        Dim jsonString = Await task.Content.ReadAsStringAsync()
        If task.IsSuccessStatusCode Then
            Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonString)
            gvAllReviews.DataSource = table
            gvAllReviews.DataBind()
        End If

    End Sub
    'convert below method To Get product by id
    Public Async Sub btnProductID_ClickAsync(sender As Object, e As EventArgs) Handles btnProductID.Click

        Dim uri As String = "https://localhost:44368/api/product/" & tbProductID.Value
        Dim task = Await httpClient.GetAsync(uri)
        Dim jsonString As String = Await task.Content.ReadAsStringAsync()
        If task.IsSuccessStatusCode Then
            Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonString)
            gvProductID.DataSource = table
            gvProductID.DataBind()
        End If

    End Sub

    Public Async Sub btnReviewID_ClickAsync(sender As Object, e As EventArgs) Handles btnReviewID.Click

        Dim uri As String = "https://localhost:44368/api/review/" & tbReviewID.Value
        Dim task = Await httpClient.GetAsync(uri)
        Dim jsonString As String = Await task.Content.ReadAsStringAsync()
        If task.IsSuccessStatusCode Then
            Dim table As DataTable = JsonConvert.DeserializeObject(Of DataTable)(jsonString)
            gvReviewID.DataSource = table
            gvReviewID.DataBind()
        End If

    End Sub

    'convert below method to create product
    Private Async Sub btnCreateProduct_ClickAsync(sender As Object, e As EventArgs) Handles btnCreateProduct.Click
        'I think i need to create a product class. make a product object on page load and place the textbox values into that object then serialize it to json
        'Dim createdProduct As Product
        Dim createdProduct As New Product

        createdProduct.ProductID = CInt(tbCreateProductId.Value.Trim)
        createdProduct.ProductNo = tbCreateProductId.Value
        createdProduct.ProductName = tbProductName.Value
        createdProduct.ProductDesc = tbProductDescription.Value
        createdProduct.UnitPrice = Decimal.Parse(tbPrice.Value.Trim)
        createdProduct.MainCategoryID = CInt(tbMainCategoryID.Value.Trim)
        createdProduct.SubCategoryID = CInt(tbSubCategoryID.Value.Trim)
        createdProduct.MainCategoryName = tbMainCategoryName.Value
        createdProduct.SubCategoryName = tbSubCategoryName.Value
        createdProduct.ProductCaption = tbProductCaption.Value
        createdProduct.ProductRating = CInt(tbProductRating.Value.Trim)
        createdProduct.FeaturedProduct = CInt(tbFeatured.Value.Trim)
        createdProduct.ProductInfo = tbProductInfo.Value

        Dim json As String = JsonConvert.SerializeObject(createdProduct, Formatting.Indented)

        System.Diagnostics.Debug.WriteLine(json)
        'System.Diagnostics.Debug.WriteLine(createdProduct.MainCategoryID.GetType)
        'System.Diagnostics.Debug.WriteLine("Value of object createdProduct.ProductID: " + createdProduct.ProductID)
        'Dim myJson As String = ("{'productID': " & CInt(tbCreateProductId.Value) & ", 'productNo': '" & tbProductNo.Value & "', 'productName': '" _
        '& tbProductName.Value & "', 'productDesc': '" & tbProductDescription.Value & "', 'unitPrice': " & CInt(tbPrice.Value) & ", 'mainCategoryID': " & CInt(tbMainCategoryID.Value) & ", 'subCategoryID': " & CInt(tbSubCategoryID.Value) & ", 'mainCategoryName': '" & tbMainCategoryName.Value & "', 'SubCategoryName': '" & tbSubCategoryName.Value & "', 'productCaption': '" & tbProductCaption.Value & "', 'productRating': " & CInt(tbProductRating.Value) & ", 'featuredProduct': " & CInt(tbFeatured.Value) & ", 'productInfo': '" & tbProductInfo.Value & "'}")
        'this line prints the json that is getting sent to the api. right now it is not passing a json validator.
        'System.Diagnostics.Debug.WriteLine("json posted to created product: " + myJson)

        httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", getToken())
        Dim uri As String = "https://localhost:44368/api/product/"
        Dim response = Await httpClient.PostAsync(uri, New StringContent(json, Encoding.UTF8, "application/json"))
        btnAllProducts_ClickAsync(btnAllProducts, EventArgs.Empty)
    End Sub

    Private Async Sub btnCreateReview_ClickAsync(sender As Object, e As EventArgs) Handles btnCreateReview.Click
        Dim myJson As String = ("{'ProductID': '" & tbRProductId.Value & "', 'UserName': '" & tbUserName.Value & "', 'Rating': '" _
            & tbRating.Value & "', 'UserReview': '" & tbUserReview.Value & "'}")

        httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", getToken())
        Dim uri As String = "https://localhost:44368/api/review/"
        Dim response = Await httpClient.PostAsync(uri, New StringContent(myJson, Encoding.UTF8, "application/json"))
        btnAllReviews_ClickAsync(btnAllReviews, EventArgs.Empty)
    End Sub

    Private Async Sub btnUpdateReview_ClickAsync(sender As Object, e As EventArgs) Handles btnUpdateReview.Click
        Dim myJson As String = ("{'ProductID': '" & tbRProductId.Value & "', 'UserName': '" & tbUserName.Value & "', 'Rating': '" _
            & tbRating.Value & "', 'UserReview': '" & tbUserReview.Value & "'}")

        httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", getToken())
        Dim uri As String = "https://localhost:44368/api/review/" & tbUpdateReviewID.Value
        Dim response = Await httpClient.PutAsync(uri, New StringContent(myJson, Encoding.UTF8, "application/json"))
        btnAllReviews_ClickAsync(btnAllReviews, EventArgs.Empty)
        btnReviewID_ClickAsync(btnReviewID, EventArgs.Empty)
    End Sub

    Private Async Sub btnDeleteReviewID_ClickAsync(sender As Object, e As EventArgs) Handles btnDeleteReviewID.Click
        httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", getToken())
        Dim uri As String = "https://localhost:44368/api/review/" & tbDeleteReviewID.Value
        Await httpClient.DeleteAsync(uri)
        btnAllReviews_ClickAsync(btnAllReviews, EventArgs.Empty)
        btnReviewID_ClickAsync(btnReviewID, EventArgs.Empty)
    End Sub

    Private Async Sub btnDeleteProductID_ClickAsync(sender As Object, e As EventArgs) Handles btnDeleteProductID.Click
        httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", getToken())
        Dim uri As String = "https://localhost:44368/api/product/" & tbDeleteProductID.Value
        Await httpClient.DeleteAsync(uri)
        btnAllProducts_ClickAsync(btnAllProducts, EventArgs.Empty)
        btnProductID_ClickAsync(btnProductID, EventArgs.Empty)
    End Sub

    Private Async Sub btnImportAllReviews_ClickAsync(sender As Object, e As EventArgs) Handles btnImportAllReviews.Click
        Dim uri As String = "https://localhost:44368/api/review"
        Dim task = Await httpClient.GetAsync(uri)
        Dim jsonString = Await task.Content.ReadAsStringAsync()

        Dim sqlDr As SqlDataReader
        Dim strSQLStatement As String
        Dim cmdSQL As SqlCommand
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionStringOnlineStore").ConnectionString
        Dim conn As New SqlConnection(strConnectionString)
        conn.Open()
        strSQLStatement = "DECLARE @json NVARCHAR(max) SET @json = N'" & jsonString & "'; INSERT INTO Review  SELECT * FROM OPENJSON(@json) WITH (productID int, userName varchar(30), rating int, userReview varchar(300))"
        cmdSQL = New SqlCommand(strSQLStatement, conn)
        sqlDr = cmdSQL.ExecuteReader()
        conn.Close()
    End Sub
    Function getToken() As String
        Dim jwtToken As String
        If (Request.Cookies("JwtCookie") IsNot Nothing) Then
            If (Request.Cookies("JwtCookie")("JWT") IsNot Nothing) Then
                jwtToken = Request.Cookies("JwtCookie")("JWT")
                Return jwtToken
            End If
        End If
        Return Nothing
    End Function


End Class