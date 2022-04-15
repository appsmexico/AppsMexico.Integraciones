# AppsMexico.Integraciones
Proyectos con ejemplos para realizar integraciones con servicios de Apps Mexico
<div class="dw-content-page ">

<div class="dw-content">

# Archivos para pruebas de timbrado y cancelación

<div class="level1">

En nuestra plataforma de pruebas se tienen varios parámetros modificados para el uso de la plataforma de pruebas, dos de ellos son el uso de los certificados y los <abbr title="Request for Comments">RFC</abbr>'s en los XML de pruebas, aquí podrá encontrar el <abbr title="Request for Comments">RFC</abbr> a utilizar, este cuenta con su par de archivos que conforman el CSD.

Se debe de registrar este <abbr title="Request for Comments">RFC</abbr> de pruebas el cual sera el emisor, dentro del panel DEMO.

* * *

![:!:](/lib/images/smileys/icon_exclaim.gif) **Nuevos certificados**: Para realizar pruebas de timbrado de CFDI puede utilizar los siguientes <abbr title="Request for Comments">RFC</abbr>'s con su certificado correspondiente, la contraseña de la llave privada es la siguiente: **12345678a**

* * *

<span style="color:#00a2e8; ">**Lista de <abbr title="Request for Comments">RFC</abbr> para utilizarse como Emisores en CFDI 4.0**</span>

<div class="table-responsive">

<table class="inline table table-striped table-condensed">

<thead>

<tr class="row0">

<th class="col0 leftalign"><abbr title="Request for Comments">RFC</abbr></th>

<th class="col1">Nombre</th>

<th class="col2">Domicilio fiscal</th>

<th class="col3 centeralign">Certificados</th>

<th class="col4 centeralign">Estatus</th>

<th class="col5 centeralign">Estímulo Fronterizo</th>

</tr>

</thead>

<tbody>

<tr class="row1">

<td class="col0 leftalign">EKU9003173C9</td>

<td class="col1">ESCUELA KEMPER URGATE</td>

<td class="col2">20928, 20914, 21855, 31508</td>

<td class="col3 centeralign">[csd_eku9003173c9_20190617131829.zip]</td>

<td class="col4 centeralign">**✓**</td>

<td class="col5 centeralign">**✓**</td>

</tr>

<tr class="row2">

<td class="col0 leftalign">IIA040805DZ4</td>

<td class="col1">INDISTRIA ILUMINADORA DE ALMACENES</td>

<td class="col2">64258, 63900, 62964</td>

<td class="col3 centeralign">[csd_iia040805dz4_20190617133200.zip]</td>

<td class="col4 centeralign">**✓**</td>

<td class="col5 centeralign">**✓**</td>

</tr>

<tr class="row3">

<td class="col0 leftalign">IVD920810GU2</td>

<td class="col1">INNOVACION VALOR Y DESARROLLO</td>

<td class="col2">61957, 61100, 58000</td>

<td class="col3 centeralign">[csd_ivd920810gu2_20190617133525.zip]</td>

<td class="col4 centeralign">**✓**</td>

<td class="col5 centeralign">**×**</td>

</tr>

<tr class="row4">

<td class="col0 leftalign">MISC491214B86</td>

<td class="col1">CECILIA MIRANDA SANCHEZ</td>

<td class="col2">64258, 63900</td>

<td class="col3 centeralign">[csd_misc491214b86_20190528175539.zip]</td>

<td class="col4 centeralign">**✓**</td>

<td class="col5 centeralign">**✓**</td>

</tr>

<tr class="row5">

<td class="col0 leftalign">XIQB891116QE4</td>

<td class="col1">BERENICE XIMO QUEZADA</td>

<td class="col2">57424</td>

<td class="col3 centeralign">[csd_xiqb891116qe4_20190528180227.zip]</td>

<td class="col4 centeralign">**✓**</td>

<td class="col5 centeralign">**×**</td>

</tr>

</tbody>

</table>

</div>

* * *

<span style="color:#00a2e8; ">**Lista de <abbr title="Request for Comments">RFC</abbr> para utilizarse como RECEPTORES en CFDI 3.3 y CFDI 4.0**</span>

<div class="table-responsive">

<table class="inline table table-striped table-condensed">

<thead>

<tr class="row0">

<th class="col0 leftalign"><abbr title="Request for Comments">RFC</abbr></th>

<th class="col1">Nombre/Razón social</th>

<th class="col2">Domicilio fiscal</th>

<th class="col3">Régimen Fiscal</th>

<th class="col4">Tipo persona</th>

</tr>

</thead>

<tbody>

<tr class="row1">

<td class="col0">MASO451221PM4</td>

<td class="col1 leftalign">MARIA OLIVIA MARTINEZ SAGAZ</td>

<td class="col2">80290</td>

<td class="col3">605, 606, 607, 608, 610, 611, 612, 614, 615, 616, 621, 625, 626</td>

<td class="col4">Física</td>

</tr>

<tr class="row2">

<td class="col0">AABF800614HI0</td>

<td class="col1">FELIX MANUEL ANDRADE BALLADO</td>

<td class="col2">86400</td>

<td class="col3">605, 606, 607, 608, 610, 611, 612, 614, 615, 616, 621, 625, 626</td>

<td class="col4">Física</td>

</tr>

<tr class="row3">

<td class="col0">CUSC850516316</td>

<td class="col1">CESAR OSBALDO CRUZ SOLORZANO</td>

<td class="col2">45638</td>

<td class="col3">605, 606, 607, 608, 610, 611, 612, 614, 615, 616, 621, 625, 626</td>

<td class="col4">Física</td>

</tr>

<tr class="row4">

<td class="col0">ICV060329BY0</td>

<td class="col1">INMOBILIARIA</td>

<td class="col2">33826</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

<tr class="row5">

<td class="col0">ABC970528UHA</td>

<td class="col1 leftalign">ARENA BLANCA</td>

<td class="col2">80290</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

<tr class="row6">

<td class="col0">CTE950627K46</td>

<td class="col1">COMERCIALIZADORA TEODORIKAS</td>

<td class="col2">57740</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

<tr class="row7">

<td class="col0">AMO8905171T1</td>

<td class="col1">ALBERCAS MONTAÑO</td>

<td class="col2">22000</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

<tr class="row8">

<td class="col0">GCA000415UX7</td>

<td class="col1">GRUPO DE CONSTRUCCION ARQUITECTONICA NACIONAL</td>

<td class="col2">11830</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

<tr class="row9">

<td class="col0 leftalign">HHN0507087N4</td>

<td class="col1">HIDRO HORTICOLA DEL NOROESTE</td>

<td class="col2">82198</td>

<td class="col3">601, 603, 610, 620, 622, 623, 624, 626</td>

<td class="col4">Moral</td>

</tr>

</tbody>

</table>

</div>

</div>

</div>

</div>
